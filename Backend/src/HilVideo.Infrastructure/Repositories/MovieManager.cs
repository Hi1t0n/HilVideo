using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;
using UserService.Domain.Contracts;
using UserService.Domain.DTO.MovieDTO;
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
    
    /// <summary>
    /// Получение всех фильмов 
    /// </summary>
    /// <returns>Список фильмов (Для рендеринга карточек фильмов на клиенте)</returns>
    public async Task<Result<List<GetMoviesResponse>, IError>> GetSearchMoviesAsync(string movieName)
    {
        if (string.IsNullOrWhiteSpace(movieName))
        {
            return Result.Failure<List<GetMoviesResponse>, IError>(new BadRequestError("Введите название фильма"));
        }

        movieName = char.ToUpper(movieName[0]) + movieName[1..];
        
        var movies = await _context.Movies
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
            .Where(x=> EF.Functions.Like(x.MovieName, $"%{movieName}%"))
            .Select(m => new GetMoviesResponse(
                m.MovieId,
                m.MovieName,
                m.MovieDescription,
                m.PosterFilePath,
                m.MovieType.MovieTypeName,
                m.ReleaseDate,
                m.MovieDirectors
                    .Select(md => $"{md.Director!.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres
                    .Select(mg => mg.Genre!.GenreName)
                    .ToList()
            )).AsNoTracking().ToListAsync();

        if (!movies.Any())
        {
            return Result.Failure<List<GetMoviesResponse>, IError>(new NotFoundError($"Фильм с названием {movieName} не найден"));
        }
        
        return Result.Success<List<GetMoviesResponse>, IError>(movies);
    }

    public async Task<Result<List<GetMoviesResponse>>> GetFavoriteMoviesByUserIdAsync(Guid userId)
    {
        var movies = await _context.Movies.Include(x=> x.MovieDirectors)
                .ThenInclude(x=> x.Director)
            .Include(x=> x.MovieGenres)
                .ThenInclude(x=> x.Genre)
            .Where(x=> x.FavoriteMoviesUsers.Any(fmu=> fmu.UserId == userId))
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
                        $"{md.Director!.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres.Select(mg => mg.Genre!.GenreName).ToList()
            )).AsNoTracking().ToListAsync();
        
        return Result.Success(movies);
    }

    public async Task<Result<List<GetMoviesResponse>>> GetMoviesAsync()
    {
        var movies = await _context.Movies
            .Include(m => m.MovieDirectors)
                .ThenInclude(md => md.Director)
            .Include(m => m.MovieGenres)
                .ThenInclude(mg => mg.Genre)
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
                    .Select(md => $"{md.Director!.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
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
                    .Select(md => $"{md.Director!.SecondName} {md.Director.FirstName} {md.Director.Patronymic}")
                    .ToList(),
                m.MovieGenres.Select(mg => mg.Genre!.GenreName).ToList()
            )).AsNoTracking().FirstOrDefaultAsync();

        if (movie is null)
        {
            return Result.Failure<GetMovieByIdResponse, IError>(new NotFoundError($"Фильм c Id: {id} не найден!"));
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
            return Result.Failure<Movie, IError>(new NotFoundError($"Фильм с Id: {id} не найлен"));
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
            return Result.Failure<Movie, IError>(new NotFoundError($"Фильм с Id: {request.MovieId} не найден"));
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
            }));
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
            }));
        }

        _context.Update(movie);
        await _context.SaveChangesAsync();


        return Result.Success<Movie, IError>(movie);
    }
    
    /// <summary>
    /// Добавление фильма в избранные пользователем
    /// </summary>
    /// <param name="request">Данные для добавления</param>
    /// <returns>Результат операции</returns>
    public async Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteRequest request)
    {
        await _context.FavoriteMoviesUsers.AddAsync(new FavoriteMoviesUsers()
        {
            UserId = request.userId,
            MovieId = request.movieId
        });

        await _context.SaveChangesAsync();

        return Result.Success();
    }
    
    /// <summary>
    /// Удаление фильма из избранных у пользователя
    /// </summary>
    /// <param name="request">Данные для добавления</param>
    /// <returns>Результат операции</returns>
    public async Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieFromFavoritesAsync(MovieToFavoriteRequest request)
    {
        var existData = await _context.FavoriteMoviesUsers
            .Where(fum => fum.UserId == request.userId && fum.MovieId == request.movieId).FirstOrDefaultAsync();
        if (existData is null)
        {
            return Result.Failure<FavoriteMoviesUsers, IError>(new NotFoundError("Запись не найдена"));
        }

        _context.FavoriteMoviesUsers.Remove(existData);
        await _context.SaveChangesAsync();
        return Result.Success<FavoriteMoviesUsers, IError>(existData);
    }

    public async Task<Result<bool>> CheckMovieFromFavoritesAsync(CheckMovieFromFavoritesRequest request)
    {
        var movieToFavorite = await _context.FavoriteMoviesUsers
            .Where(x => x.UserId == request.UserId && x.MovieId == request.MovieId).FirstOrDefaultAsync();

        return movieToFavorite is null ? Result.Success(false) : Result.Success(true);
    }

    public async Task<Result<List<GetMovieIdWithName>>> GetMovieIdWithNameAsync()
    {
        var movies = await _context.Movies.Select(x => new GetMovieIdWithName(x.MovieId, x.MovieName)).ToListAsync();

        return Result.Success(movies);
    }
}