using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Contracts.GenreDTO;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

/// <summary>
///     Реализация интерфейса <see cref="IGenreManager"/>
/// </summary>
public class GenreManager : IGenreManager
{
    private readonly ApplicationDbContext _context;
    
    /// <inheritdoc cref="IGenreManager"/>
    public GenreManager(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task<Result<Genre, IError >> AddGenreAsync(string? genreName)
    {
        if (string.IsNullOrWhiteSpace(genreName))
        {
            return Result.Failure<Genre, IError>(new BadRequestError("Название введено некорректно"));
        }
        
        var entry =  await _context.Genres.AddAsync(new Genre()
        {
            GenreId = Guid.NewGuid(),
            GenreName = genreName
        });

        await _context.SaveChangesAsync();

        return Result.Success<Genre, IError>(entry.Entity);
    }
    
    /// <inheritdoc />
    public async Task<Result<List<GenreResponse>>> GetAllGenresAsync()
    {
        var genres = await _context.Genres.Select(x => new GenreResponse(x.GenreId, x.GenreName)).ToListAsync();

        return Result.Success(genres);
    }

    /// <inheritdoc />
    public async Task<Result<Genre, IError>> UpdateGenreByIdAsync(UpdateGenreRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewName))
        {
            return Result.Failure<Genre, IError>(new BadRequestError("Название введено некорректно"));
        }

        var genre = await _context.Genres.Where(x => x.GenreId == request.Id).FirstOrDefaultAsync();

        if (genre is null)
        {
            return Result.Failure<Genre, IError>(new NotFoundError("Жанр не найден"));
        }

        genre.GenreName = request.NewName;

        _context.Genres.Update(genre);
        await _context.SaveChangesAsync();

        return Result.Success<Genre, IError>(genre);

    }

    /// <inheritdoc />
    public async Task<Result<Genre, IError>> DeleteGenreByIdAsync(Guid id)
    {
        var genre = await _context.Genres.Where(x => x.GenreId == id).FirstOrDefaultAsync();

        if (genre is null)
        {
            return Result.Failure<Genre, IError>(new NotFoundError("Жанр не найден"));
        }
        
        var moviesgenres = await _context.MoviesGenres.Where(x => x.GenreId == id).ToListAsync();

        if (moviesgenres.Any())
        {
            _context.MoviesGenres.RemoveRange(moviesgenres);
        }
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();

        return Result.Success<Genre, IError>(genre);
    }
}