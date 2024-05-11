using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Contracts.MovieTypeDTO;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

/// <summary>
/// Реализация интерфейса <see cref="IMovieTypeManager"/>
/// </summary>
public class MovieTypeManager : IMovieTypeManager  
{
    private readonly ApplicationDbContext _context;
    
    /// <inheritdoc cref="IMovieTypeManager"/>
    public MovieTypeManager(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task<Result<MovieType, IError>> AddMovieTypeAsync(string movieTypeName)
    {
        if (string.IsNullOrWhiteSpace(movieTypeName))
        {
            return Result.Failure<MovieType, IError>(new BadRequestError("Введите название"));
        }

        var entry = await _context.MovieTypes.AddAsync(new MovieType
        {
            MovieTypeId = Guid.NewGuid(),
            MovieTypeName = movieTypeName
        });

        await _context.SaveChangesAsync();
        
        return Result.Success<MovieType, IError>(entry.Entity);
    }
    
    /// <inheritdoc />
    public async Task<Result<List<MovieTypeResponse>>> GetAllMovieTypeAsync()
    {
        var movieTypes = await _context.MovieTypes
            .Select(x => new MovieTypeResponse(x.MovieTypeId, x.MovieTypeName))
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(movieTypes);
    }

    /// <inheritdoc />
    public async Task<Result<MovieType, IError>> UpdateMovieTypeByIdAsync(MovieTypeUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewMovieTypeName))
        {
            return Result.Failure<MovieType, IError>(new BadRequestError("Введите название"));
        }
        
        var movieType = await _context.MovieTypes.FirstOrDefaultAsync(x => x.MovieTypeId == request.Id);

        if (movieType is null)
        {
            return Result.Failure<MovieType, IError>(new NotFoundError("Тип не найден"));
        }

        movieType.MovieTypeName = request.NewMovieTypeName;
        _context.MovieTypes.Update(movieType);
        await _context.SaveChangesAsync();

        return Result.Success<MovieType, IError>(movieType);
    }

    /// <inheritdoc />
    public Task<Result<MovieType, IError>> DeleteMovieTypeByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}