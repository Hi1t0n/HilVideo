using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Interfaces;

namespace Infrastructure.Helpers;

public class FileLoader : IFileLoader
{
    private const string VideoFilePath = @"..\..\..\data\Movies\"; /* Путь для сохранения фильмов */
    private const string ImageFilePath = @"..\..\..\data\Posters\"; /* Путь для сохранения фильмов */
    private IWordsTranslate _wordsTranslate;
    public FileLoader(IWordsTranslate wordsTranslate)
    {
        _wordsTranslate = wordsTranslate;
    }
    
    /// <summary>
    /// Сохранение видео файлов 
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="movieName">Название фильма</param>
    /// <returns>Полный путь по которому сохранен файл</returns>
    public async Task<Result<string>> LoadVideoFileAsync(IFormFile? file, string movieName)
    {
        if (file is null || file.Length == 0)
        {
            return Result.Failure<string>("File not load");
        }

        string filePath = Path.Combine(VideoFilePath, 
            $"{_wordsTranslate.WordTranslate(movieName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Result.Success(Path.GetFullPath(filePath));
    }
    
    /// <summary>
    /// Сохранение файлов картинок
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="movieName">Название фильма</param>
    /// <returns>Полный путь по которому сохранен файл</returns>
    public async Task<Result<string>> LoadImageFileAsync(IFormFile? file, string movieName)
    {
        if (file is null || file.Length == 0)
        {
            return Result.Failure<string>("File not load");
        }

        string filePath = Path.Combine(ImageFilePath,
            $"{_wordsTranslate.WordTranslate(movieName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Result.Success(Path.GetFullPath(filePath));
    }
}