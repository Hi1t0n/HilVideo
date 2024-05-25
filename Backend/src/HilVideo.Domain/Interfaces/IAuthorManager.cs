using CSharpFunctionalExtensions;
using UserService.Domain.DTO.AuthorDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IAuthorManager
{
     /// <summary>
     /// Добавление автора
     /// </summary>
     /// <param name="request">Данные об авторе</param>
     /// <returns>Результат добавления с данными автора или ошибка</returns>
     Task<Result<Author, IError>>  AddAuthorAsync(AddAuthorRequest request);
     
     /// <summary>
     /// Получение всех авторов
     /// </summary>
     /// <returns>Список всех авторов</returns>
     Task<Result<List<AuthorResponse>>> GetAllAuthorAsync();

     /// <summary>
     /// Обновление автора по id
     /// </summary>
     /// <param name="request">Данные автора</param>
     /// <returns>Результат обновления с данными автора или ошибка</returns>
     Task<Result<Author, IError>> UpdateAuthorByIdAsync(UpdateAuthorRequest request);

     /// <summary>
     /// Удаление автора по id
     /// </summary>
     /// <param name="id">Идентификатор удаляемового автора</param>
     /// <returns>Результат удаления с данными автора или ошибка</returns>
     Task<Result<Author, IError>> DeleteAuthorByIdAsync(Guid id);
}