using CSharpFunctionalExtensions;
using UserService.Domain.Contracts.DirectorDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IDirectorManager
{
    /// <summary>
    /// Добавление режиссёра
    /// </summary>
    /// <param name="request">Данные режиссёра</param>
    /// <returns>Результат добавления и объект <see cref="Director"/></returns>
    Task<Result<Director, IError>> AddDirectorAsync(AddDirectorRequest request);
    
    /// <summary>
    /// Получение всех режиссёров
    /// </summary>
    /// <returns>Список всех режиссёров</returns>
    Task<Result<List<DirectorResponse>>> GetAllDirectorsAsync();
    
    /// <summary>
    /// Обновление данных режиссёра
    /// </summary>
    /// <param name="request">Новые данные режиссёра</param>
    /// <returns>Результат обновления и объект <see cref="Director"/></returns>
    Task<Result<Director, IError>> UpdateDirectorByIdAsync(UpdateDirectorRequest request);
    
    /// <summary>
    /// Удаление режиссёра
    /// </summary>
    /// <param name="id">Идентификатор удаляемого режиссёра</param>
    /// <returns>Результат удаления и объект <see cref="Director"/></returns>
    Task<Result<Director, IError>> DeleteDirectorByIdAsync(Guid id);
}