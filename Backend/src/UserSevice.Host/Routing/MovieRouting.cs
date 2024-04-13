using Microsoft.AspNetCore.Mvc;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;

namespace UserSevice.Host.Routing;

public static class MovieRouting
{
    public static WebApplication AddMovieRouting(this WebApplication application)
    {
        var movieGroup = application.MapGroup("/api/movie");

        movieGroup.MapPost(pattern: "/", handler: AddMovie).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        movieGroup.MapPost(pattern: "/addmovietofavorites", handler: AddMovieToFavorites).RequireAuthorization();
        movieGroup.MapGet(pattern: "/", handler: GetAllMovies);
        movieGroup.MapGet(pattern: "/{id:guid}", handler: GetMovieById);
        movieGroup.MapDelete(pattern: "/deletemoviefromfavorites", handler: DeleteMovieFromFavorites).RequireAuthorization();
        
        return application;
    }

    public static async Task<IResult> AddMovie(AddMovieRequest request, IMovieManager movieManager, HttpContext context)
    {
        AddMovieWithFileRequest data = new
        (
            MovieName: request.MovieName,
            MovieDescription: request.MovieDescription,
            MovieType: request.MovieType,
            ReleaseData: request.ReleaseDate,
            PosterFile: context.Request.Form.Files.GetFile("poster"),
            MovieFile: context.Request.Form.Files.GetFile("Movies"),
            Directors: request.Directors,
            Genres: request.Genres
        );
        
        var result = await movieManager.AddMovieAsync(data);

        switch (result.Error)
        {
            case BadRequestResult error:
                return Results.BadRequest(new
                {
                    error = result.Error.ErrorMessange
                });
        }
        
        
        return Results.Created();
    }

    public static async Task<IResult> GetAllMovies([FromQuery] string url, IMovieManager movieManager)
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

    public static async Task<IResult> GetMovieById(Guid id, IMovieManager movieManager)
    {
        var result = await movieManager.GetMovieByIdAsync(id);

        switch (result.Error)
        {
            case NotFoundError error:
                return Results.NotFound(new
                {
                    error = error.ErrorMessange
                });
        }

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> AddMovieToFavorites(MovieToFavoriteRequest request, IMovieManager movieManager)
    {
        var result = await movieManager.AddMovieToFavoritesAsync(request);

        return Results.Ok();
    }
    
    //TODO: API Обновления
    public static async Task<IResult> UpdateMovieById(Guid id)
    {
        throw new Exception();
    }
    
    //TODO: API Удаления
    public static async Task<IResult> DeleteMovieById(Guid id)
    {
        throw new Exception();
    }

    public static async Task<IResult> DeleteMovieFromFavorites([FromBody] MovieToFavoriteRequest request,
       IMovieManager movieManager)
    {
        var result = await movieManager.DeleteMovieFromFavoritesAsync(request);

        switch (result.Error)
        {
            case NotFoundError error:
                return Results.NotFound(new
                {
                    error = error.ErrorMessange
                });
        }

        return Results.Ok();
    }
}