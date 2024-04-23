using CSharpFunctionalExtensions;
using UserService.Domain.Contracts.DirectorDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IDirectorManager
{
    Task<Result<Director, IError>> AddDirectorAsync(AddDirectorRequest request);
    Task<Result<List<DirectorResponse>>> GetAllDirectorsAsync();
    Task<Result<Director, IError>> UpdateDirectorByIdAsync(UpdateDirectorRequest request);
    Task<Result<Director, IError>> DeleteDirectorByIdAsync(Guid id);
}