using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Contracts.DirectorDTO;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

/// <summary>
///     Реализация интерфейса <see cref="IUserManager"/>
/// </summary>
public class DirectorManager : IDirectorManager
{

    private readonly ApplicationDbContext _context;
    
    /// <inheritdoc cref="IUserManager"/>
    public DirectorManager(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task<Result<Director, IError>> AddDirectorAsync(AddDirectorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.SecondName))
        {
            return Result.Failure<Director, IError>(new BadRequestError("Введены некорректные данные"));
        }

        Director director = new()
        {
            DirectorId = Guid.NewGuid(),
            FirstName = request.FirstName,
            SecondName = request.SecondName,
            Patronymic = request.Patronymic
        };

        await _context.Directors.AddAsync(director);
        await _context.SaveChangesAsync();

        return Result.Success<Director, IError>(director);

    }
    
    /// <inheritdoc />
    public async Task<Result<List<DirectorResponse>>> GetAllDirectorsAsync()
    {
        var directors = await _context.Directors
            .Select(x => new DirectorResponse(x.DirectorId, x.FirstName, x.SecondName, x.Patronymic)).AsNoTracking().ToListAsync();

        return Result.Success(directors);
    }
    
    /// <inheritdoc />
    public async Task<Result<Director, IError>> UpdateDirectorByIdAsync(UpdateDirectorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.SecondName))
        {
            return Result.Failure<Director, IError>(new BadRequestError("Введены некорректные данные"));
        }

        var director = await _context.Directors.Where(x => x.DirectorId == request.Id).FirstOrDefaultAsync();

        if (director is null)
        {
            return Result.Failure<Director, IError>(new NotFoundError("Режиссер не найден"));
        }

        director.FirstName = request.FirstName;
        director.SecondName = request.SecondName;
        director.Patronymic = request.Patronymic;

        _context.Directors.Update(director);
        await _context.SaveChangesAsync();

        return Result.Success<Director, IError>(director);
    }
    
    /// <inheritdoc />
    public async Task<Result<Director, IError>> DeleteDirectorByIdAsync(Guid id)
    {
        var director = await _context.Directors.Where(x => x.DirectorId == id).FirstOrDefaultAsync();

        if (director is null)
        {
            return Result.Failure<Director, IError>(new NotFoundError("Режиссер не найден"));
        }

        var moviesDirectors = await _context.MoviesDirectors.Where(x => x.DirectorId == id).ToListAsync();

        if (moviesDirectors.Any())
        {
            _context.MoviesDirectors.RemoveRange(moviesDirectors);
        }

        _context.Directors.Remove(director);
        await _context.SaveChangesAsync();

        return Result.Success<Director, IError>(director);
    }
}