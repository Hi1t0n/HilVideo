using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using UserService.Domain.Contracts.DirectorDTO;
using UserService.Domain.Contracts.GenreDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace UserSevice.Host.Routing;

public static class DirectorRouting
{
    public static WebApplication AddDirectorRouting(this WebApplication application)
    {
        var directorGroup = application.MapGroup("/api/directors");

        directorGroup.MapPost(pattern: "/", handler: AddDirectorAsync);
        directorGroup.MapGet(pattern: "/", handler: GetAllDirectorsAsync);
        directorGroup.MapPut(pattern: "/", handler: UpdateDirectorByIdAsync);
        directorGroup.MapDelete(pattern: "/", handler: DeleteDirectorByIdAsync);

        return application;
    }

    public static async Task<IResult> AddDirectorAsync(AddDirectorRequest request, IDirectorManager manager)
    {
        var result = await manager.AddDirectorAsync(request);

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

    public static async Task<IResult> GetAllDirectorsAsync(IDirectorManager manager)
    {
        var result = await manager.GetAllDirectorsAsync();

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdateDirectorByIdAsync(UpdateDirectorRequest request, IDirectorManager manager)
    {
        var result = await manager.UpdateDirectorByIdAsync(request);

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

    public static async Task<IResult> DeleteDirectorByIdAsync(Guid id, IDirectorManager manager)
    {
        var result = await manager.DeleteDirectorByIdAsync(id);

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