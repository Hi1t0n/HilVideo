using CSharpFunctionalExtensions;
using UserService.Domain.DTO.BookDTO;
using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

/// <summary>
/// Интерфейс для взаимодействия с книгами
/// </summary>
public interface IBookManager
{
    /// <summary>
    /// Добавление книги
    /// </summary>
    /// <param name="request">Данные для добавления книги</param>
    /// <returns>Результат добавление с данными или ошибка</returns>
    Task<Result<AddBookRequest, IError>> AddBookAsync(AddBookRequest request);
    
    /// <summary>
    /// Поиск книги по параметра
    /// </summary>
    /// <param name="bookName">Данные для поиска</param>
    /// <returns>Данные книги</returns>
    Task<Result<List<GetBooksResponse>, IError>> GetSearchBookAsync(string bookName);
    
    /// <summary>
    /// Получение всех избранных книг пользователь по id
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Все избранные книги пользователя</returns>
    Task<Result<List<GetBooksResponse>>> GetFavoriteBooksByUserIdAsync(Guid userId);
    
    /// <summary>
    /// Получение списка книг
    /// </summary>
    /// <returns>Результат и данные книг</returns>
    Task<Result<List<GetBooksResponse>>> GetBooksAsync();
    
    /// <summary>
    /// Получение данных книги по id
    /// </summary>
    /// <param name="id">Идентификатор книги</param>
    /// <returns>Данные книги или ошибка</returns>
    Task<Result<GetBookByIdResponse, IError>> GetBookByIdAsync(Guid id);
    
    /// <summary>
    /// Обновление данных книги по id
    /// </summary>
    /// <param name="request">Данные для обновления</param>
    /// <returns>Результат обновления</returns>
    Task<Result<Book, IError>> UpdateBookByIdAsync(UpdateBookRequest request);
    
    /// <summary>
    /// Удаление книги по id
    /// </summary>
    /// <param name="id">Идентификатор книги</param>
    /// <returns>Результат удаления</returns>
    Task<Result<Book, IError>> DeleteBookByIdAsync(Guid id);
    
    /// <summary>
    /// Добавление книги в избранное
    /// </summary>
    /// <param name="request">Данные для добавления</param>
    /// <returns>Результат добавления</returns>
    Task<Result> AddBookToFavoritesAsync(BookToFavoriteRequest request);
    
    /// <summary>
    /// Удаление книги из избранного
    /// </summary>
    /// <param name="request">Данные для удаления</param>
    /// <returns>Результат удаления</returns>
    Task<Result<FavoriteBooksUsers, IError>> DeleteBookFromFavoritesAsync(BookToFavoriteRequest request);

    /// <summary>
    /// Проверка содержится ли книга у пользователя в избранном
    /// </summary>
    /// <param name="request">DTO для получения</param>
    /// <returns>Булевое значение</returns>
    Task<Result<bool>> CheckBookFromFavoritesAsync(CheckBookFromFavoritesRequest request);
    
    /// <summary>
    /// Получение списка с id и названием книги
    /// </summary>
    /// <returns>Список книг</returns>
    Task<Result<List<GetBookIdWithName>>> GetBookIdWithNameAsync();
}