using CSharpFunctionalExtensions;
using UserService.Domain.Contracts;
using UserService.Domain.Models;
using UserService.Infrastructure.Enums;

namespace UserService.Domain.Interfaces;

public interface IMovieManager
{
    Task<Result<AddMovieWithFileRequest, IError>> AddMovieAsync(AddMovieWithFileRequest withFileRequest );
    Task<Result<List<GetMoviesResponse>>> GetAllMoviesAsync(MovieSearchRequest request);
    Task<Result<GetMovieByIdResponse, IError>> GetMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> DeleteMovieByIdAsync(Guid id);
    Task<Result<Movie, IError>> UpdateMovieByIdAsync(Guid id);
    Task<Result> AddMovieToFavoritesAsync(MovieToFavoriteDTO data);
    Task<Result<FavoriteMoviesUsers, IError>> DeleteMovieToFavoritesAsync(MovieToFavoriteDTO data);
    Task<Result<IError, string>> GetFilePathByIdAsync(Guid id);

}