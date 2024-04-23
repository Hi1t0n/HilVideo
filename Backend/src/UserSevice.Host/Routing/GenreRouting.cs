using Microsoft.AspNetCore.Http.HttpResults;
using UserService.Domain.Contracts.GenreDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;

namespace UserSevice.Host.Routing;

/// <summary>
///     Роутер для работы с жанрами
/// </summary>
public static class GenreRouting
{
    /// <summary>
    ///     Добавление роутера для работы с жанрами
    /// </summary>
    /// <param name="application">Объект приложения</param>
    /// <returns>Модифицированный объект приложения</returns>
    public static WebApplication AddGenreRouting(this WebApplication application)
    {
        var genreGroup = application.MapGroup("/api/genres");

        genreGroup.MapPost(pattern: "/", handler: AddGenreAsync);
        genreGroup.MapGet(pattern: "/", handler: GetAllGenresAsync);
        genreGroup.MapPut(pattern: "/", handler: UpdateGenreByIdAsync);
        genreGroup.MapDelete(pattern: "/", handler: DeleteGenreByIdAsync);
        
        return application;
    }
    
    /// <summary>
    ///     Добавление жанра
    /// </summary>
    /// <param name="genreName">Название жанра</param>
    /// <param name="manager"><see cref="IGenreManager"/></param>
    /// <returns>Результат добавления</returns>
    public static async Task<IResult> AddGenreAsync(string genreName, IGenreManager manager)
    {
        var result = await manager.AddGenreAsync(genreName);

        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }

    /// <summary>
    ///     Получение всех жанров
    /// </summary>
    /// <param name="manager"><see cref="IGenreManager"/></param>
    /// <returns>Список всех жанров</returns>
    public static async Task<IResult> GetAllGenresAsync(IGenreManager manager)
    {
        var result = await manager.GetAllGenresAsync();

        return Results.Ok(result.Value);
    }
    
    /// <summary>
    /// Обновление жанра
    /// </summary>
    /// <param name="request">Данные для обновления жанра</param>
    /// <param name="manager"><see cref="IGenreManager"/></param>
    /// <returns>Результат обновления</returns>
    public static async Task<IResult> UpdateGenreByIdAsync(UpdateGenreRequest request, IGenreManager manager)
    {
        var result = await manager.UpdateGenreByIdAsync(request);

        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case BadRequestError error:
                    return Results.BadRequest(new
                    {
                        error = error.ErrorMessange
                    });
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }
    
    /// <summary>
    ///     Удаление жанра
    /// </summary>
    /// <param name="id">Идентификатор удаляемого жанра</param>
    /// <param name="manager"><see cref="IGenreManager"/></param>
    /// <returns>Результат удаления</returns>
    public static async Task<IResult> DeleteGenreByIdAsync(Guid id, IGenreManager manager)
    {
        var result = await manager.DeleteGenreByIdAsync(id);

        if (result.IsFailure)
        {
            switch (result.Error)
            {
                case NotFoundError error:
                    return Results.NotFound(new
                    {
                        error = error.ErrorMessange
                    });
            }
        }

        return Results.Ok();
    }
    
}