using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;

namespace UserSevice.Host.Routing;

public static class MovieRouting
{
    public static WebApplication AddMovieRouting(this WebApplication application)
    {
        
        // var authGroup = application.MapGroup("/api/auth");
        // authGroup.MapPost(pattern: "/register",handler: Register);
        var movieGroup = application.MapGroup("/api/movie");

        movieGroup.MapGet(pattern: "/", handler: GetAllMovies);
        
        return application;
    }

    public static async Task<IResult> GetAllMovies([FromQuery] string url, [FromServices]IMovieManager movieManager)
    {
        if (MovieSearchRequest.TryParse(url, out MovieSearchRequest request))
        {
            var movies = await movieManager.GetAllMoviesAsync(request);

            return Results.Ok(movies.Value);
        }
        else
        {
            return Results.BadRequest("Неверные параметры поиска");
        }
    }
}