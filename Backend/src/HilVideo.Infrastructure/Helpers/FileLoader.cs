using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Interfaces;


namespace AuthService.Infrastructure.Helpers;

public class FileLoader : IFileLoader
{
    private const string VideoFilePath = @"..\..\..\..\..\data\Movies\"; /* Путь для сохранения фильмов */
    private const string ImageFilePath = @"..\..\..\..\..\data\Posters\"; /* Путь для сохранения фильмов */
    
    /// <summary>
    /// Сохранение видео файлов 
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="movieName">Название фильма</param>
    /// <returns>Полный путь по которому сохранен файл</returns>
    public async Task<Result<string>> LoadVideoFileAsync(IFormFile file, string movieName)
    {
        if (file is null || file.Length == 0)
        {
            return Result.Failure<string>("File not load");
        }

        string filePath = Path.Combine(VideoFilePath, 
            $"{WordsTranslate.WordTranslate(movieName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Result.Success(filePath);
    }
    
    /// <summary>
    /// Сохранение файлов картинок
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="movieName">Название фильма</param>
    /// <returns>Полный путь по которому сохранен файл</returns>
    public async Task<Result<string>> LoadImageFileAsync(IFormFile file, string movieName)
    {
        if (file is null || file.Length == 0)
        {
            return Result.Failure<string>("File not load");
        }

        string filePath = Path.Combine(ImageFilePath,
            $"{WordsTranslate.WordTranslate(movieName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Result.Success(filePath);
    }
}