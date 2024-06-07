using CSharpFunctionalExtensions;
using UserService.Domain.Contracts;
using UserService.Domain.DTO.MovieDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IMovieManager
{
    Task<Result<AddMovieRequest, IError>> AddMovieAsync(AddMovieRequest request );
    Task<Result<List<GetMoviesResponse>, IError>> GetSearchMoviesAsync(string movieName);
    Task<Result<List<GetMoviesResponse>>> GetFavoriteMoviesByUserIdAsync(Guid userId);
    Task<Result<List<GetMoviesResponse>>> GetMoviesAsync();
    Task<Result<GetMovieByIdResponse, IError>> GetMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> UpdateMovieByIdAsync(UpdateMovieRequest request);
    Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteRequest request);
    Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieFromFavoritesAsync(MovieToFavoriteRequest request);
    Task<Result<bool>> CheckMovieFromFavoritesAsync(CheckMovieFromFavoritesRequest request);
    Task<Result<List<GetMovieIdWithName>>> GetMovieIdWithNameAsync();

}