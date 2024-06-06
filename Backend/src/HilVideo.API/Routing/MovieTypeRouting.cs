using UserService.Domain.Contracts.MovieTypeDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;

namespace UserSevice.Host.Routing;

public static class MovieTypeRouting
{
    public static WebApplication AddMovieTypeRouting(this WebApplication application)
    {
        var movieTypeGroup = application.MapGroup("/api/movietype");

        movieTypeGroup.MapPost(pattern: "/", handler: AddMovieTypeAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        movieTypeGroup.MapGet(pattern: "/", handler: GetAllMovieTypeAsync);
        movieTypeGroup.MapPut(pattern: "/", handler: UpdateMovieTypeByIdAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");;
        
        return application;
    }

    public static async Task<IResult> AddMovieTypeAsync(string movieTypeName, IMovieTypeManager manager)
    {
        var result = await manager.AddMovieTypeAsync(movieTypeName);

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

    public static async Task<IResult> GetAllMovieTypeAsync(IMovieTypeManager manager)
    {
        var result = await manager.GetAllMovieTypeAsync();
        return Results.Ok(result.Value);
    }

    public static async Task<IResult> UpdateMovieTypeByIdAsync(MovieTypeUpdateRequest request,
        IMovieTypeManager manager)
    {
        var result = await manager.UpdateMovieTypeByIdAsync(request);

        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
                
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }
}