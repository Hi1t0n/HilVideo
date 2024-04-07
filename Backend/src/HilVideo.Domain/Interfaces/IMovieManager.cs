using CSharpFunctionalExtensions;
using UserService.Domain.Contracts;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IMovieManager
{
    Task<Result<List<GetMoviesResponse>>> GetAllMoviesAsync();
    Task<Result<GetMovieByIdResponse, IError>> GetMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> UpdateMovieByIdAsync(Guid id);
    Task<Result<IError>> AddMovieToFavoritesAsync(Guid userId, Guid movieId);
    Task<Result<IError>> DeleteMovieToFavoritesAsync(Guid userId, Guid movieId);

}