using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.DTO.BookDTO;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;
/// <summary>
/// Реализация интерфейса <see cref="IBookManager"/>
/// </summary>
public class BookManager : IBookManager
{
    private readonly ApplicationDbContext _context;
    private readonly IFileHelper _fileHelper;
    private readonly ISorting _sorting;
    
    private readonly int _getBook = 9;
    
    /// <see cref="IBookManager"/>
    public BookManager(ApplicationDbContext context, IFileHelper fileHelper, ISorting sorting)
    {
        _context = context;
        _fileHelper = fileHelper;
        _sorting = sorting;
    }
    
    /// <inheritdoc />
    public async Task<Result<AddBookRequest, IError>> AddBookAsync(AddBookRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BookName))
        {
            return Result.Failure<AddBookRequest, IError>(new BadRequestError("Введите название книги"));
        }

        if (string.IsNullOrWhiteSpace(request.BookDescription))
        {
            return Result.Failure<AddBookRequest, IError>(new BadRequestError("Введите описание книги"));
        }

        if (!request.AuthorsId.Any())
        {
            return Result.Failure<AddBookRequest, IError>(new BadRequestError("Выберите автора"));
        }

        if (!request.GenresId.Any())
        {
            return Result.Failure<AddBookRequest, IError>(new BadRequestError("Выберите жанр"));
        }

        var bookFilePath = await _fileHelper.LoadBookFileAsync(request.BookFile, request.BookName);

        if (bookFilePath.IsFailure)
        {
            return Result.Failure<AddBookRequest, IError>(new BadRequestError($"{bookFilePath.Error}"));
        }

        var posterFilePath = await _fileHelper.LoadImageFileAsync(request.PosterFile, request.BookName);

        if (bookFilePath.IsFailure)
        {
            return Result.Failure<AddBookRequest, IError>(new BadRequestError($"{posterFilePath.Error}"));
        }

        await _context.Database.BeginTransactionAsync();

        try
        {
            var book = new Book
            {
                BookId = Guid.NewGuid(),
                BookName = request.BookName,
                BookDescription = request.BookDescription,
                PosterFilePath = posterFilePath.Value,
                BookFilePath = bookFilePath.Value,
                ReleaseDate = request.ReleaseDate
            };

            await _context.Books.AddAsync(book);

            var bookAuthors = request.AuthorsId.Select(id => new BookAuthor
            {
                BookId = book.BookId,
                AuthorId = id
            }).ToList();

            await _context.BookAuthors.AddRangeAsync(bookAuthors);

            var bookGenres = request.GenresId.Select(id => new BookGenre
            {
                BookId = book.BookId,
                GenreId = id
            });

            await _context.BookGenres.AddRangeAsync(bookGenres);
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();

            return Result.Success<AddBookRequest, IError>(request);

        }
        catch (Exception e)
        {
            await _context.Database.RollbackTransactionAsync();
            return Result.Failure<AddBookRequest, IError>(new BadRequestError("Что-то пошло не так"));
        }
    }
    
    /// <inheritdoc />
    public async Task<Result<List<GetBooksResponse>, IError>> GetSearchBookAsync(string bookName)
    {
        if (string.IsNullOrWhiteSpace(bookName))
        {
            return Result.Failure<List<GetBooksResponse>, IError>(new BadRequestError("Введите название книги"));
        }
        
        bookName = char.ToUpper(bookName[0]) + bookName[1..];
        
        var books = await _context.Books.Include(x => x.BookGenres)
                .ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
            .Where(x=> EF.Functions.Like(x.BookName, $"%{bookName}%"))
            .Select(x => new GetBooksResponse
            (
                x.BookId,
                x.BookName,
                x.BookDescription,
                x.PosterFilePath,
                x.BookFilePath,
                x.ReleaseDate,
                x.BookAuthors
                    .Select(bookAuthor => $"{bookAuthor.Author!.SecondName} {bookAuthor.Author.FirstName} {bookAuthor.Author.Patronymic}")
                    .ToList(),
                x.BookGenres
                    .Select(bookGenre => bookGenre.Genre!.GenreName)
                    .ToList()
            )).AsNoTracking().ToListAsync();

        if (!books.Any())
        {
            return Result.Failure<List<GetBooksResponse>, IError>(new NotFoundError($"Книга с названием {bookName} не найдена"));
        }
        
        return Result.Success<List<GetBooksResponse>, IError>(books);
    }
    
    ///<inheritdoc />
    public async Task<Result<List<GetBooksResponse>>> GetFavoriteBooksByUserIdAsync(Guid userId)
    {
        var books = await _context.Books.Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
            .Include(x => x.BookGenres)
                .ThenInclude(x => x.Genre)
            .Where(x=> x.FavoriteBooksUsers.Any(fbu=> fbu.UserId == userId))
            .Select(x => new GetBooksResponse
            (
                x.BookId,
                x.BookName,
                x.BookDescription,
                x.PosterFilePath,
                x.BookFilePath,
                x.ReleaseDate,
                x.BookAuthors.Select(bookAuthor => $"{bookAuthor.Author!.SecondName} {bookAuthor.Author.FirstName} {bookAuthor.Author.Patronymic}").ToList(),
                x.BookGenres.Select(bookGenre => bookGenre.Genre!.GenreName).ToList()
            )).ToListAsync();

        return Result.Success(books);
    }

    /// <inheritdoc />
    public async Task<Result<List<GetBooksResponse>>> GetBooksAsync()
    {
        var books = await _context.Books.Include(x => x.BookGenres)
                .ThenInclude(x => x.Genre)
            .Include(x => x.BookAuthors)
                .ThenInclude(x => x.Author)
            .OrderByDescending(x=> x.ReleaseDate)
            .Select(x => new GetBooksResponse
            (
                x.BookId,
                x.BookName,
                x.BookDescription,
                x.PosterFilePath,
                x.BookFilePath,
                x.ReleaseDate,
                x.BookAuthors
                    .Select(bookAuthor =>
                        $"{bookAuthor.Author!.SecondName} {bookAuthor.Author.FirstName} {bookAuthor.Author.Patronymic}")
                    .ToList(),
                x.BookGenres
                    .Select(bookGenre => bookGenre.Genre!.GenreName)
                    .ToList()
            )).Take(_getBook).AsNoTracking().ToListAsync();

        return Result.Success(books);
    }

    /// <inheritdoc />
    public async Task<Result<GetBookByIdResponse, IError>> GetBookByIdAsync(Guid id)
    {
        var book = await _context.Books.Include(x => x.BookAuthors)
            .ThenInclude(x => x.Author)
            .Include(x => x.BookGenres)
            .ThenInclude(x => x.Genre)
            .Where(x => x.BookId == id)
            .Select(x => new GetBookByIdResponse
            (
                x.BookId,
                x.BookName,
                x.BookDescription,
                x.BookFilePath,
                x.PosterFilePath,
                x.ReleaseDate,
                x.BookAuthors
                    .Select(bookAuthor => $"{bookAuthor.Author!.SecondName} {bookAuthor.Author.FirstName} {bookAuthor.Author.Patronymic}")
                    .ToList(),
                x.BookGenres
                    .Select(bookGenre => bookGenre.Genre!.GenreName)
                    .ToList()
            )).AsNoTracking().FirstOrDefaultAsync();

        if (book is null)
        {
            return Result.Failure<GetBookByIdResponse, IError>(new NotFoundError($"Книга с Id: {id} не найдена"));
        }

        return Result.Success<GetBookByIdResponse, IError>(book);
    }

    /// <inheritdoc />
    public async Task<Result<Book, IError>> UpdateBookByIdAsync(UpdateBookRequest request)
    {
        var book = await _context.Books.Include(x => x.BookAuthors)
            .Include(x => x.BookGenres)
            .Where(x => x.BookId == request.BookId)
            .FirstOrDefaultAsync();

        if (book is null)
        {
            return Result.Failure<Book, IError>(new NotFoundError($"Книга с Id: {request.BookId} не найдена"));
        }

        book.BookName = request.BookName;
        book.BookDescription = request.BookDescription;
        book.ReleaseDate = request.ReleaseDate;

        if (request.RemovedGenresId?.Any() is true)
        {
            book.BookGenres.RemoveAll(x => request.RemovedGenresId.Contains(x.GenreId));
        }

        if (request.AddedGenresId?.Any() is true)
        {
            book.BookGenres.AddRange(request.AddedGenresId.Select(id=> new BookGenre
            {
                BookId = request.BookId,
                GenreId = id
            }));
        }

        if (request.RemovedAuthorsId?.Any() is true)
        {
            book.BookAuthors.RemoveAll(x => request.RemovedAuthorsId.Contains(x.AuthorId));
        }

        if (request.AddedAuthorsId?.Any() is true)
        {
            book.BookAuthors.AddRange(request.AddedAuthorsId.Select(id=> new BookAuthor
            {
                BookId = request.BookId,
                AuthorId = id
            }));
        }

        _context.Update(book);
        await _context.SaveChangesAsync();

        return Result.Success<Book, IError>(book);
    }

    /// <inheritdoc />
    public async Task<Result<Book, IError>> DeleteBookByIdAsync(Guid id)
    {
        var book = await _context.Books.Where(x => x.BookId == id).FirstOrDefaultAsync();

        if (book is null)
        {
            return Result.Failure<Book, IError>(new NotFoundError($"Книга с Id: {id} не найдена"));
        }
        
        var bookFiles = new List<string>
        {
            book.BookFilePath,
            book.PosterFilePath
        };
        
        _fileHelper.DeleteFilesByPath(bookFiles);

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return Result.Success<Book, IError>(book);
    }

    /// <inheritdoc />
    public async Task<Result> AddBookToFavoritesAsync(BookToFavoriteRequest request)
    {
        await _context.AddAsync(new FavoriteBooksUsers
        {
            UserId = request.UserId,
            BookId = request.BookId
        });

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result<FavoriteBooksUsers, IError>> DeleteBookFromFavoritesAsync(BookToFavoriteRequest request)
    {
        var bookFavorite = await _context.FavoriteBooksUsers
            .Where(x => x.UserId == request.UserId && x.BookId == request.BookId).FirstOrDefaultAsync();

        if (bookFavorite is null)
        {
            return Result.Failure<FavoriteBooksUsers, IError>(new 
                NotFoundError($"Книга с Id: {request.BookId} у пользователя с Id: {request.UserId} в избранном не найдена"));
        }
        
        _context.FavoriteBooksUsers.Remove(bookFavorite);
        await _context.SaveChangesAsync();

        return Result.Success<FavoriteBooksUsers, IError>(bookFavorite);
    }
}