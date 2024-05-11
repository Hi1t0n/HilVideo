using CSharpFunctionalExtensions;
using UserService.Domain.Contracts;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IMovieManager
{
    Task<Result<AddMovieRequest, IError>> AddMovieAsync(AddMovieRequest request );
    Task<Result<List<GetMoviesResponse>>> GetAllMoviesAsync(MovieSearchRequest request);
    Task<Result<GetMovieByIdResponse, IError>> GetMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> UpdateMovieByIdAsync(UpdateMovieRequest request);
    Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteRequest data);
    Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieFromFavoritesAsync(MovieToFavoriteRequest data);
    Task<Result<IError, string>> GetFilePathByIdAsync(Guid id);

}