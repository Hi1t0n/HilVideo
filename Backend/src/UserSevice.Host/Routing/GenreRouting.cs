using Microsoft.AspNetCore.Http.HttpResults;
using UserService.Domain.Contracts.GenreDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;

namespace UserSevice.Host.Routing;

public static class GenreRouting
{
    public static WebApplication AddGenreRouting(this WebApplication application)
    {
        var genreGroup = application.MapGroup("/api/genres");

        genreGroup.MapPost(pattern: "/", handler: AddGenreAsync);
        genreGroup.MapGet(pattern: "/", handler: GetAllGenresAsync);
        genreGroup.MapPut(pattern: "/", handler: UpdateGenreByIdAsync);
        genreGroup.MapDelete(pattern: "/", handler: DeleteGenreByIdAsync);
        
        return application;
    }

    public static async Task<IResult> AddGenreAsync(string genreName, IGenreManager manager)
    {
        var result = await manager.AddGenreAsync(genreName);

        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }

    public static async Task<IResult> GetAllGenresAsync(IGenreManager manager)
    {
        var result = await manager.GetAllGenresAsync();

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdateGenreByIdAsync(UpdateGenreRequest request, IGenreManager manager)
    {
        var result = await manager.UpdateGenreByIdAsync(request);

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

    public static async Task<IResult> DeleteGenreByIdAsync(Guid id, IGenreManager manager)
    {
        var result = await manager.DeleteGenreByIdAsync(id);

        if (result.IsFailure)
        {
            switch (result.Error)
            {
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