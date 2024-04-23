using CSharpFunctionalExtensions;
using UserService.Domain.Contracts.GenreDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IGenreManager
{
    Task<Result<Genre, IError>> AddGenreAsync(string genreName);
    Task<Result<List<GenreResponse>>> GetAllGenresAsync();
    Task<Result<Genre, IError>> UpdateGenreByIdAsync(UpdateGenreRequest request);
    Task<Result<Genre, IError>> DeleteGenreByIdAsync(Guid id);
}