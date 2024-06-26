using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace UserService.Domain.Interfaces;

public interface IFileHelper
{
    Task<Result<string>> LoadVideoFileAsync(IFormFile? file, string movieName);
    Task<Result<string>> LoadImageFileAsync(IFormFile? file, string movieName);
    Task<Result<string>> LoadBookFileAsync(IFormFile? file, string bookName);
    void DeleteFilesByPath(List<string> pathFiles);
}