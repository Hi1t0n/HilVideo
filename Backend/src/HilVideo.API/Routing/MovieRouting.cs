using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserService.Domain.Contracts;
using UserService.Domain.DTO.MovieDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace UserSevice.Host.Routing;

public static class MovieRouting
{
    public static WebApplication AddMovieRouting(this WebApplication application)
    {
        var movieGroup = application.MapGroup("/api/movie");

        movieGroup.MapPost(pattern: "/", handler: AddMovie).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        movieGroup.MapPost(pattern: "/addmovietofavorites", handler: AddMovieToFavorites).RequireAuthorization();
        movieGroup.MapGet(pattern: "/search/{movieName}", handler: GetSearchMovies);
        movieGroup.MapGet(pattern: "/getfavoritemovies/{id:guid}", handler: GetFavoriteMoviesByUserIdAsync);
        movieGroup.MapGet(pattern: "/", handler: GetMoviesAsync);
        movieGroup.MapGet(pattern: "/{id:guid}", handler: GetMovieById);
        movieGroup.MapGet(pattern: "/check-favorite", handler: CheckMovieFromFavoritesAsync).RequireAuthorization();
        movieGroup.MapPut(pattern: "/", handler: UpdateMovieById).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        movieGroup.MapDelete(pattern: "/{id:guid}", handler: DeleteMovieById).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        movieGroup.MapDelete(pattern: "/deletemoviefromfavorites", handler: DeleteMovieFromFavorites).RequireAuthorization();
        
        return application;
    }

    public static async Task<IResult> AddMovie(IMovieManager movieManager, HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var movieName = form["MovieName"];
        var movieDescription = form["MovieDescription"];
        var movieType = Guid.Parse(form["MovieType"]!);
        var releaseDate = DateTime.Parse(form["ReleaseDate"]!).ToUniversalTime();
        var directors = JsonConvert.DeserializeObject<List<Guid>>(form["Directors"]);
        var genres = JsonConvert.DeserializeObject<List<Guid>>(form["Genres"]);
        var posterFile = form.Files.GetFile("Poster");
        var movieFile = form.Files.GetFile("Movie");



        AddMovieRequest data = new AddMovieRequest
        (
            movieName,
            movieDescription,
            movieType,
            releaseDate,
            posterFile,
            movieFile,
            directors,
            genres
        );
        
        var result = await movieManager.AddMovieAsync(data);

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
        
        
        return Results.Created();
    }

    public static async Task<IResult> GetSearchMovies(string movieName, IMovieManager movieManager)
    {
        var result = await movieManager.GetSearchMoviesAsync(movieName);
        
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

    public static async Task<IResult> GetFavoriteMoviesByUserIdAsync(Guid id, IMovieManager movieManager)
    {
        var result = await movieManager.GetFavoriteMoviesByUserIdAsync(id);

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetMoviesAsync(IMovieManager movieManager)
    {
        var result = await movieManager.GetMoviesAsync();

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> GetMovieById(Guid id, IMovieManager movieManager)
    {
        var result = await movieManager.GetMovieByIdAsync(id);

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

        return Results.Ok(result.Value);
    }

    public static async Task<IResult> AddMovieToFavorites(MovieToFavoriteRequest request, IMovieManager movieManager)
    {
        var result = await movieManager.AddMovieToFavoritesAsync(request);

        return Results.Ok();
    }
    
    public static async Task<IResult> UpdateMovieById(UpdateMovieRequest request, IMovieManager movieManager)
    {
        var result = await movieManager.UpdateMovieByIdAsync(request);

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

        return Results.Ok(result.Value);
    }
    
    public static async Task<IResult> DeleteMovieById(Guid id, IMovieManager movieManager)
    {
        var result = await movieManager.DeleteMovieByIdAsync(id);

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

    public static async Task<IResult> DeleteMovieFromFavorites([FromBody] MovieToFavoriteRequest request,
       IMovieManager movieManager)
    {
        var result = await movieManager.DeleteMovieFromFavoritesAsync(request);

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

    public static async Task<IResult> CheckMovieFromFavoritesAsync(Guid userId, Guid movieId,
        IMovieManager movieManager)
    {
        var result = await movieManager.CheckMovieFromFavoritesAsync(new CheckMovieFromFavoritesRequest(userId, movieId));

        return Results.Ok(new
        {
            isFavorite = result.Value
        });
    }
}