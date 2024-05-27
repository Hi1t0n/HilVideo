using System;
using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using UserService.Domain.Interfaces;

namespace Infrastructure.Helpers;

public class FileHelper : IFileHelper
{
    private const string VideoFilePath = @"..\..\..\data\Movies\"; /* Путь для сохранения фильмов */
    private const string ImageFilePath = @"..\..\..\data\Posters\"; /* Путь для сохранения картинок */
    private const string BookFilePath = @"..\..\..\data\Books"; /* Путь для сохранения книг */
    private const string MainFilePath = @"C:\Diplom\";
    private IWordsTranslate _wordsTranslate;
    public FileHelper(IWordsTranslate wordsTranslate)
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
            $"{_wordsTranslate.WordTranslate(movieName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.mp4");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Result.Success(Path.GetRelativePath(@"C:\Diplom\",filePath));
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
            $"{_wordsTranslate.WordTranslate(movieName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Result.Success(Path.GetRelativePath(@"C:\Diplom\",filePath));
    }

    public async Task<Result<string>> LoadBookFileAsync(IFormFile? file, string bookName)
    {
        if (file is null || file.Length == 0)
        {
            return Result.Failure<string>("File not load");
        }

        string filePath = Path.Combine(BookFilePath,
            $"{_wordsTranslate.WordTranslate(bookName)}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf");

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        return Result.Success(Path.GetRelativePath(@"C:\Diplom\",filePath));
    }

    public void DeleteFilesByPath(List<string> pathFiles)
    {
        foreach (var path in pathFiles)
        {
            if (File.Exists($"{MainFilePath}{path}"))
            {
                File.Delete($"{MainFilePath}{path}");
            }
        }
    }
}