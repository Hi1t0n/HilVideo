using CSharpFunctionalExtensions;
using UserService.Domain.Contracts.MovieTypeDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

/// <summary>
///     Интерфейс взаимодействия с типами фильма
/// </summary>
public interface IMovieTypeManager
{
    /// <summary>
    ///     Добавление нового типа
    /// </summary>
    /// <param name="movieTypeName">Название типа</param>
    /// <returns>Результат</returns>
    Task<Result<MovieType, IError>> AddMovieTypeAsync(string movieTypeName);
    
    /// <summary>
    ///     Получение всех типов
    /// </summary>
    /// <returns>Результат и список</returns>
    Task<Result<List<MovieTypeResponse>>> GetAllMovieTypeAsync();
    
    /// <summary>
    /// Обновление типа по id 
    /// </summary>
    /// <param name="request">DTO Id - идентификатор нужного типа | NewMovieTypeName - новое название
    /// </param>
    /// <returns></returns>
    Task<Result<MovieType, IError>> UpdateMovieTypeByIdAsync(MovieTypeUpdateRequest request);

    Task<Result<MovieType, IError>> DeleteMovieTypeByIdAsync(Guid id);
}