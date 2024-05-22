using CSharpFunctionalExtensions;
using Infrastructure.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Enums;
using UserService.Infrastructure.ErrorObjects;
using UserService.Domain.Contracts;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Repositories;

public class MovieManager : IMovieManager
{
    private readonly ApplicationDbContext _context;
    private readonly IFileHelper _fileHelper;
    private readonly ISorting _sorting;

    private readonly int _getMovie = 9;
    public MovieManager(ApplicationDbContext context, IFileHelper fileHelper, ISorting sorting)
    {
        _context = context;
        _fileHelper = fileHelper;
        _sorting = sorting;
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

        var movieFilePath = await _fileHelper.LoadVideoFileAsync(request.MovieFile, request.MovieName);
        if (movieFilePath.IsFailure)
        {
            return Result.Failure<AddMovieRequest, IError>(new BadRequestError($"{movieFilePath.Error}"));
        }
        
        var posterFilePath = await _fileHelper.LoadImageFileAsync(request.PosterFile, request.MovieName);
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
                    ReleaseDate = request.ReleaseData
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
    
    //TODO: Пагинация?
    /// <summary>
    /// Получение всех фильмов 
    /// </summary>
    /// <returns>Список фильмов (Для рендеринга карточек фильмов на клиенте)</returns>
    public async Task<Result<List<GetMoviesResponse>>> GetSearchMoviesAsync(MovieSearchRequest request)
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
        
        query = _sorting.ApplySorting(query, request.SortBy);
       

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

    public async Task<Result<List<GetMoviesResponse>>> GetMoviesAsync()
    {
        var movies = await _context.Movies
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieType)
                .ThenInclude(mt => mt.Movies)
            .OrderByDescending(m=>m.ReleaseDate)
            .Select(m => new GetMoviesResponse
            (
                m.MovieId,
                m.MovieName,
                m.MovieDescription,
                m.PosterFilePath,
                m.MovieType.MovieTypeName,
                m.ReleaseDate,
                m.MovieDirectors
                    .Select(md =>
                        $"{md.Director.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres.Select(mg => mg.Genre!.GenreName).ToList()
            )).AsNoTracking().Take(_getMovie).ToListAsync();
        return Result.Success(movies);
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
        var movie = await _context.Movies
            .Include(x => x.MovieFiles)
            .FirstOrDefaultAsync(x => x.MovieId == id);
        if (movie is null)
        {
            return Result.Failure<Movie, IError>(new NotFoundError("Фильм не найлен"));
        }

        var movieFiles = movie.MovieFiles.Select(x => x.FilePath).ToList();
        movieFiles.Add(movie.PosterFilePath);
        _fileHelper.DeleteFilesByPath(movieFiles);
        
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
        return Result.Success<Movie, IError>(movie);
    }

    public async Task<Result<Movie, IError>> UpdateMovieByIdAsync(UpdateMovieRequest request)
    {
        var movie = await _context.Movies
            .Include(m => m.MovieGenres)
            .Include(m => m.MovieDirectors)
            .Include(m => m.MovieFiles)
            .Where(m => m.MovieId == request.MovieId)
            .FirstOrDefaultAsync();

        if (movie is null)
        {
            return Result.Failure<Movie, IError>(new NotFoundError("Обновляемый фильм не найден"));
        }

        movie.MovieName = request.MovieName;
        movie.MovieDescription = request.MovieDescription;
        movie.ReleaseDate = request.ReleaseDate;
        movie.MovieTypeId = request.MovieType;
        
        // Удаление жанров
        if (request.RemovedGenresId is not null)
        {
            movie.MovieGenres.RemoveAll(mg => request.RemovedGenresId.Contains(mg.GenreId));
        }
        
        // Добавление жанров
        if (request.AddedGenresId is not null)
        { 
            movie.MovieGenres.AddRange(request.AddedGenresId.Select(g => new MovieGenre()
            {
                MovieId = request.MovieId,
                GenreId = g
            }).ToList());
        }
        
        // Удаление режиссеров
        if (request.RemovedDirectorsId is not null)
        {
            movie.MovieDirectors.RemoveAll(md=> request.RemovedDirectorsId.Contains(md.DirectorId));
        }
        
        // Добавление режиссеров
        if (request.AddedDirectorsId is not null)
        {
            movie.MovieDirectors.AddRange(request.AddedDirectorsId.Select(d=> new MovieDirector()
            {
                MovieId = request.MovieId,
                DirectorId = d
            }).ToList());
        }

        await _context.SaveChangesAsync();


        return Result.Success<Movie, IError>(movie);
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
}