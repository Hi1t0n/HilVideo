using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace UserService.Domain.Interfaces;

public interface IFileLoader
{
    Task<Result<string>> LoadVideoFileAsync(IFormFile? file, string movieName);
    Task<Result<string>> LoadImageFileAsync(IFormFile? file, string movieName);
}