using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.DTO.AuthorDTO;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

/// <summary>
/// Реализация интерфейса <see cref="IAuthorManager"/>
/// </summary>
public class AuthorManager : IAuthorManager
{
    private readonly ApplicationDbContext _context;
    
    /// <inheritdoc cref="IAuthorManager"/>
    public AuthorManager(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <inheritdoc />
    public async Task<Result<Author, IError>> AddAuthorAsync(AddAuthorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return Result.Failure<Author, IError>(new BadRequestError("Введите имя"));
        }
        
        if (string.IsNullOrWhiteSpace(request.SecondName))
        {
            return Result.Failure<Author, IError>(new BadRequestError("Введите фамилию"));
        }

        Author author = new()
        {
            AuthorId = Guid.NewGuid(),
            FirstName = request.FirstName,
            SecondName = request.SecondName,
            Patronymic = request.Patronymic
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        return Result.Success<Author,IError>(author);
    }

    /// <inheritdoc />
    public async Task<Result<List<AuthorResponse>>> GetAllAuthorAsync()
    {
        var authors = await _context.Authors
            .Select(x => new AuthorResponse(x.AuthorId, $"{x.SecondName} {x.FirstName} {x.Patronymic}")).ToListAsync();

        return Result.Success(authors);
    }

    public async Task<Result<Author, IError>> UpdateAuthorByIdAsync(UpdateAuthorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return Result.Failure<Author, IError>(new BadRequestError("Введите имя"));
        }
        
        if (string.IsNullOrWhiteSpace(request.SecondName))
        {
            return Result.Failure<Author, IError>(new BadRequestError("Введите фамилию"));
        }

        var author = await _context.Authors.Where(x => x.AuthorId == request.Id).FirstOrDefaultAsync();

        if (author is null)
        {
            return Result.Failure<Author, IError>(new NotFoundError($"Автор с Id: {request.Id} не найден"));
        }

        author.FirstName = request.FirstName;
        author.SecondName = request.SecondName;
        author.Patronymic = request.Patronymic;

        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
        
        return Result.Success<Author, IError>(author); 
    }
    
    /// <inheritdoc />
    public async Task<Result<Author, IError>> DeleteAuthorByIdAsync(Guid id)
    {
        var author = await _context.Authors.Where(x => x.AuthorId == id).FirstOrDefaultAsync();

        if (author is null)
        {
            return Result.Failure<Author, IError>(new NotFoundError($"Автор с Id: {id} не найден"));
        }

        var bookAuthor = await _context.BookAuthors.Where(x => x.AuthorId == id).ToListAsync();
        
        if(bookAuthor.Any())
        {
            _context.BookAuthors.RemoveRange(bookAuthor);
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return Result.Success<Author, IError>(author);
    }
}