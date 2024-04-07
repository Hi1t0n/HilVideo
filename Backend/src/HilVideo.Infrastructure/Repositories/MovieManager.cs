using CSharpFunctionalExtensions;
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
    
    public MovieManager(ApplicationDbContext context)
    {
        _context = context;
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

    public Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Movie, IError>> UpdateMovieByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IError>> AddMovieToFavoritesAsync(Guid userId, Guid movieId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IError>> DeleteMovieToFavoritesAsync(Guid userId, Guid movieId)
    {
        throw new NotImplementedException();
    }
}