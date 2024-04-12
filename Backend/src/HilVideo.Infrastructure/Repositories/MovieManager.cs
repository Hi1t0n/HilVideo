using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

public class MovieManager : IMovieManager
{
    private readonly ApplicationDbContext _context;
    private readonly HttpContext _httpContext;
    private readonly IFileLoader _fileLoader;
    
    public MovieManager(ApplicationDbContext context, HttpContext httpContext, IFileLoader fileLoader)
    {
        _context = context;
        _httpContext = httpContext;
        _fileLoader = fileLoader;
    }
    
    /// <summary>
    /// Добавление фильма
    /// </summary>
    /// <param name="request">Данные для добавления фильма</param>
    /// <returns>Статус</returns>
    public async Task<Result<AddMovieRequest, IError>> AddMovieAsync(AddMovieRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.MovieName))
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError("Введите название фильма"));
        }
        
        if (string.IsNullOrWhiteSpace(request.MovieDescription))
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError("Введите описание"));
        }
        
        if (string.IsNullOrWhiteSpace(request.MovieType.ToString()))
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError("Выберите тип"));
        }

        if (request.Directors.Count == 0)
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError("Выберите режиссера"));
        }

        if (request.Genres.Count == 0)
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError("Выберите жанр"));
        }

        var movieFilePath = await _fileLoader.LoadVideoFileAsync(_httpContext.Request.Form.Files.GetFile("movie"), request.MovieName);
        if (movieFilePath.IsFailure)
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError($"{movieFilePath.Error}"));
        }
        
        var posterFilePath = await _fileLoader.LoadImageFileAsync(_httpContext.Request.Form.Files.GetFile("poster"), request.MovieName);
        if (movieFilePath.IsFailure)
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError($"{posterFilePath.Error}"));
        }

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                var movie = new Movie
                {
                    MovieId = Guid.NewGuid(),
                    MovieName = request.MovieName,
                    MovieDescription = request.MovieDescription,
                    PosterFilePath = posterFilePath.Value,
                    MovieTypeId = request.MovieType,
                    ReleaseDate = request.ReliseDate
                };

                await _context.Movies.AddAsync(movie);

                var movieFile = new MovieFile
                {
                    MovieFileId = Guid.NewGuid(),
                    MovieId = movie.MovieId,
                    FilePath = movieFilePath.Value
                };

                await _context.MovieFiles.AddAsync(movieFile);

                var movieGenres = request.Genres.Select(genre => new MovieGenre
                {
                    MovieId = movie.MovieId,
                    GenreId = genre
                }).ToList();

                await _context.MoviesGenres.AddRangeAsync(movieGenres);

                var movieDirectors = request.Directors.Select(director => new MovieDirector
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
                return Result.Failure<AddMovieRequest, IError>(new BadRequestError("Что-то пошло не так")); 
            }

            return Result.Success<AddMovieRequest,IError>(request);
        }
    }

    /// <summary>
    /// Получение всех фильмов 
    /// </summary>
    /// <returns>Список фильмов (Для рендеринга карточек фильмов на клиенте)</returns>
    public async Task<Result<List<GetMoviesResponse>>> GetAllMoviesAsync()
    {
        var movieList = await _context.Movies
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieType)
                .ThenInclude(mt => mt.Movies)
            .Select(m => new GetMoviesResponse
            (
                m.MovieId,
                m.MovieName,
                m.MovieDescription,
                m.PosterFilePath,
                m.MovieType.MovieTypeName,
                m.ReleaseDate,
                m.MovieDirectors
                    .Select(md => $"{md.Director.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres.Select(mg => mg.Genre.GenreName).ToList()
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
    public async Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteDTO data)
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
    public async Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieToFavoritesAsync(MovieToFavoriteDTO data)
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