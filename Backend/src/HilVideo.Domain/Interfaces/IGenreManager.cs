using CSharpFunctionalExtensions;
using UserService.Domain.Contracts.GenreDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

/// <summary>
///     Интерфейс взаимодействия с жанрами
/// </summary>
public interface IGenreManager
{   
    /// <summary>
    ///     Добавление жанра
    /// </summary>
    /// <param name="genreName">Название жанра</param>
    /// <returns>Результат добавления и объект <see cref="Genre"/></returns>
    Task<Result<Genre, IError>> AddGenreAsync(string genreName);
    
    /// <summary>
    ///     Получение списка жанров
    /// </summary>
    /// <returns>Список жанров</returns>
    Task<Result<List<GenreResponse>>> GetAllGenresAsync();
    
    /// <summary>
    ///     Обновление жанра
    /// </summary>
    /// <param name="request">Данные для обновления жанра</param>
    /// <returns>Результат обновления и объект <see cref="Genre"/></returns>
    Task<Result<Genre, IError>> UpdateGenreByIdAsync(UpdateGenreRequest request);
    
    /// <summary>
    ///     Удаление жанра
    /// </summary>
    /// <param name="id">Идентификатор удаляемого жанра</param>
    /// <returns>Результат удаления и объект <see cref="Genre"/></returns>
    Task<Result<Genre, IError>> DeleteGenreByIdAsync(Guid id);
}