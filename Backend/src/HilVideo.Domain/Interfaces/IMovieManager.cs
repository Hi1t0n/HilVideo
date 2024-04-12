using CSharpFunctionalExtensions;
using UserService.Domain.Contracts;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IMovieManager
{
    Task<Result<AddMovieRequest, IError>> AddMovieAsync(AddMovieRequest request);
    Task<Result<List<GetMoviesResponse>>> GetAllMoviesAsync();
    Task<Result<GetMovieByIdResponse, IError>> GetMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> UpdateMovieByIdAsync(Guid id);
    Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteDTO data);
    Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieToFavoritesAsync(MovieToFavoriteDTO data);
    Task<Result<IError, string>> GetFilePathByIdAsync(Guid id);

}