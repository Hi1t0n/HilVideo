using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserService.Domain.DTO.BookDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;

namespace UserSevice.Host.Routing;

public static class BookRouting
{
    public static WebApplication AddBookRouting(this WebApplication application)
    {
        var bookGroup = application.MapGroup("/api/books");

        bookGroup.MapPost(pattern: "/", handler: AddBookAsync);
        bookGroup.MapPost(pattern: "/addbooktofavorites", handler: AddBookToFavoritesAsync);
        bookGroup.MapGet(pattern: "/search{bookName}", handler: GetSearchBookAsync);
        bookGroup.MapGet(pattern: "/getfavoritebook/{id:guid}", handler: GetFavoriteBooksByUserIdAsync);
        bookGroup.MapGet(pattern: "/", handler: GetBooksAsync);
        bookGroup.MapGet(pattern: "/{id:guid}", handler: GetBookByIdAsync);
        bookGroup.MapPut(pattern: "/", handler: UpdateBookByIdAsync);
        bookGroup.MapDelete(pattern: "/{id:guid}", handler: DeleteMovieByIdAsync);
        bookGroup.MapDelete(pattern: "/deletebookfromfavorites", handler: DeleteBookFromFavoritesAsync);

        return application;
    }
    
    public static async Task<IResult> AddBookAsync(IBookManager manager, HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var bookName = form["BookName"];
        var bookDescription = form["BookDescription"];
        var releaseDate = DateTime.Parse(form["ReleaseDate"]!).ToUniversalTime();
        var authors = JsonConvert.DeserializeObject<List<Guid>>(form["Authors"]);
        var genres = JsonConvert.DeserializeObject<List<Guid>>(form["Genres"]);
        var posterFile = form.Files.GetFile("Poster");
        var bookFile = form.Files.GetFile("Book");

        AddBookRequest request = new AddBookRequest
        (
            bookName,
            bookDescription,
            releaseDate,
            posterFile,
            bookFile,
            authors,
            genres
        );
        
        var result = await manager.AddBookAsync(request);
        
        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Created();
    }

    public static async Task<IResult> GetSearchBookAsync(string bookName, IBookManager manager)
    {
        var result = await manager.GetSearchBookAsync(bookName);
        
        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetFavoriteBooksByUserIdAsync(Guid id, IBookManager manager)
    {
        var result = await manager.GetFavoriteBooksByUserIdAsync(id);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetBooksAsync(IBookManager manager)
    {
        var result = await manager.GetBooksAsync();

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetBookByIdAsync(Guid id, IBookManager manager)
    {
        var result = await manager.GetBookByIdAsync(id);
        
        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdateBookByIdAsync(UpdateBookRequest request, IBookManager manager)
    {
        var result = await manager.UpdateBookByIdAsync(request);
        
        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> DeleteMovieByIdAsync(Guid id, IBookManager manager)
    {
        var result = await manager.DeleteBookByIdAsync(id);
        
        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }

    public static async Task<IResult> AddBookToFavoritesAsync(BookToFavoriteRequest request, IBookManager manager)
    {
        var result = await manager.AddBookToFavoritesAsync(request);

        return Results.Ok();
    }

    public static async Task<IResult> DeleteBookFromFavoritesAsync([FromBody]BookToFavoriteRequest request, IBookManager manager)
    {
        var result = await manager.DeleteBookFromFavoritesAsync(request);
        
        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }
 }