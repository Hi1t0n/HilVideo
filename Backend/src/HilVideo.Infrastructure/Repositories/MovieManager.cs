using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Enums;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

public class MovieManager : IMovieManager
{
    private readonly ApplicationDbContext _context;
    private readonly IFileLoader _fileLoader;
    private readonly ISorting _sorting;
    public MovieManager(ApplicationDbContext context, IFileLoader fileLoader, ISorting sorting)
    {
        _context = context;
        _fileLoader = fileLoader;
        _sorting = sorting;
    }
    
    /// <summary>
    /// Добавление фильма
    /// </summary>
    /// <param name="withFileRequest">Данные для добавления фильма</param>
    /// <returns>Статус</returns>
    public async Task<Result<AddMovieWithFileRequest, IError>> AddMovieAsync(AddMovieWithFileRequest withFileRequest)
    {
        if (string.IsNullOrWhiteSpace(withFileRequest.MovieName))
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError("Введите название фильма"));
        }
        
        if (string.IsNullOrWhiteSpace(withFileRequest.MovieDescription))
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError("Введите описание"));
        }
        
        if (string.IsNullOrWhiteSpace(withFileRequest.MovieType.ToString()))
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError("Выберите тип"));
        }

        if (withFileRequest.Directors.Count == 0)
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError("Выберите режиссера"));
        }

        if (withFileRequest.Genres.Count == 0)
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError("Выберите жанр"));
        }

        var movieFilePath = await _fileLoader.LoadVideoFileAsync(withFileRequest.MovieFile, withFileRequest.MovieName);
        if (movieFilePath.IsFailure)
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError($"{movieFilePath.Error}"));
        }
        
        var posterFilePath = await _fileLoader.LoadImageFileAsync(withFileRequest.PosterFile, withFileRequest.MovieName);
        if (movieFilePath.IsFailure)
        {
            return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError($"{posterFilePath.Error}"));
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var movie = new Movie
                {
                    MovieId = Guid.NewGuid(),
                    MovieName = withFileRequest.MovieName,
                    MovieDescription = withFileRequest.MovieDescription,
                    PosterFilePath = posterFilePath.Value,
                    MovieTypeId = withFileRequest.MovieType,
                    ReleaseDate = withFileRequest.ReleaseData
                };

                await _context.Movies.AddAsync(movie);

                var movieFile = new MovieFile
                {
                    MovieFileId = Guid.NewGuid(),
                    MovieId = movie.MovieId,
                    FilePath = movieFilePath.Value
                };

                await _context.MovieFiles.AddAsync(movieFile);

                var movieGenres = withFileRequest.Genres.Select(genre => new MovieGenre
                {
                    MovieId = movie.MovieId,
                    GenreId = genre
                }).ToList();

                await _context.MoviesGenres.AddRangeAsync(movieGenres);

                var movieDirectors = withFileRequest.Directors.Select(director => new MovieDirector
                {
                    MovieId = movie.MovieId,
                    DirectorId = director
                }).ToList();

                await _context.MoviesDirectors.AddRangeAsync(movieDirectors);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return Result.Failure<AddMovieWithFileRequest, IError>(new BadRequestError("Что-то пошло не так")); 
            }

            return Result.Success<AddMovieWithFileRequest,IError>(withFileRequest);
        }
    }
    
    //TODO: Пагинация?
    /// <summary>
    /// Получение всех фильмов 
    /// </summary>
    /// <returns>Список фильмов (Для рендеринга карточек фильмов на клиенте)</returns>
    public async Task<Result<List<GetMoviesResponse>>> GetAllMoviesAsync(MovieSearchRequest request)
    {
        var query = _context.Movies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.MovieName))
        {
            query = query.Where(m => EF.Functions.Like(m.MovieName, $"%{request.MovieName}%"));
        }

        if (request.Genres is not null && request.Genres.Any())
        {
            query = query.Where(m => m.MovieGenres.Any(mg => request.Genres.Contains(mg.GenreId)));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (Enum.TryParse(request.SortBy, true, out SortBy sortBy))
            {
                query = _sorting.ApplySorting(query, sortBy);
            }
        }
       

        var movieList = await query
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieType)
                .ThenInclude(mt => mt.Movies)
            .Select(m => new GetMoviesResponse(
                m.MovieId,
                m.MovieName,
                m.MovieDescription,
                m.PosterFilePath,
                m.MovieType.MovieTypeName,
                m.ReleaseDate,
                m.MovieDirectors
                    .Select(md => $"{md.Director.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres
                    .Select(mg => mg.Genre.GenreName)
                    .ToList()
            )).ToListAsync();
        

        return Result.Success(movieList);
    }
    
    /// <summary>
    /// Получение всех данных фильма по Id
    /// </summary>
    /// <param name="id"> Идентификатор нужного нам фильма</param>
    /// <returns>Все данные нужного нам фильма</returns>
    public async Task<Result<GetMovieByIdResponse, IError>> GetMovieByIdAsync(Guid id)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieFiles)
                .ThenInclude(mf => mf.Movie)
            .Include(m => m.MovieType)
                .ThenInclude(mt => mt.Movies)
            .Where(x=> x.MovieId == id)
            .Select(m => new GetMovieByIdResponse
            (
                m.MovieId,
                m.MovieName,
                m.MovieDescription,
                m.MovieFiles.OrderBy(x=>x.EpisodNumber).Select(mf => new MovieFileDTO(mf.FilePath, mf.EpisodNumber)).ToList(),
                m.PosterFilePath,
                m.MovieType.MovieTypeName,
                m.ReleaseDate,
                m.MovieDirectors
                    .Select(md => $"{md.Director.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres.Select(mg => mg.Genre.GenreName).ToList()
            )).FirstOrDefaultAsync();

        if (movie is null)
        {
            return Result.Failure<GetMovieByIdResponse, IError>(new NotFoundError("Фильм не найден!"));
        }
        
        return Result.Success<GetMovieByIdResponse, IError>(movie);
    }

    public async Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Movie, IError>> UpdateMovieByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Добавление фильма в избранные пользователем
    /// </summary>
    /// <param name="data">Данные для добавления</param>
    /// <returns>Результат операции</returns>
    public async Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteRequest data)
    {
        await _context.FavoriteMoviesUsers.AddAsync(new FavoriteMoviesUsers()
        {
            UserId = data.userId,
            MovieId = data.movieId
        });

        return Result.Success();
    }
    
    /// <summary>
    /// Удаление фильма из избранных у пользователя
    /// </summary>
    /// <param name="data">Данные для добавления</param>
    /// <returns>Результат операции</returns>
    public async Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieFromFavoritesAsync(MovieToFavoriteRequest data)
    {
        var existData = await _context.FavoriteMoviesUsers
            .Where(fum => fum.UserId == data.userId && fum.MovieId == data.movieId).FirstOrDefaultAsync();
        if (existData is null)
        {
            return Result.Failure<FavoriteMoviesUsers, IError>(new NotFoundError("Запись не найдена"));
        }

        _context.FavoriteMoviesUsers.Remove(existData);
        await _context.SaveChangesAsync();
        return Result.Success<FavoriteMoviesUsers, IError>(existData);
    }
    
    /// <summary>
    /// Загрузка фильма
    /// </summary>
    /// <param name="id"> Идентификатор фильма</param>
    /// <returns>
    ///     IError - ошибка
    ///     string - путь файла на сервере для загрузки
    /// </returns>
    public async Task<Result<IError, string>> GetFilePathByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}