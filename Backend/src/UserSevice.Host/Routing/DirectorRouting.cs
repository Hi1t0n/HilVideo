using UserService.Domain.Contracts.DirectorDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace UserSevice.Host.Routing;

/// <summary>
///     Роутер для работы с режиссёрами
/// </summary>
public static class DirectorRouting
{   
    /// <summary>
    ///     Добавление роутера для работы с режиссёрами
    /// </summary>
    /// <param name="application">Объект приложения</param>
    /// <returns>Модифицированный объект приложения</returns>
    public static WebApplication AddDirectorRouting(this WebApplication application)
    {
        var directorGroup = application.MapGroup("/api/directors");

        directorGroup.MapPost(pattern: "/", handler: AddDirectorAsync);
        directorGroup.MapGet(pattern: "/", handler: GetAllDirectorsAsync);
        directorGroup.MapPut(pattern: "/", handler: UpdateDirectorByIdAsync);
        directorGroup.MapDelete(pattern: "/{id:guid}", handler: DeleteDirectorByIdAsync);

        return application;
    }
    
    /// <summary>
    ///     Добавление режиссера
    /// </summary>
    /// <param name="request">Данные добавляемого режиссера</param>
    /// <param name="manager"><see cref="IUserManager"/></param>
    /// <returns>Результат добавления</returns>
    public static async Task<IResult> AddDirectorAsync(AddDirectorRequest request, IDirectorManager manager)
    {
        var result = await manager.AddDirectorAsync(request);

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
    ///     Получение всех режиссёров
    /// </summary>
    /// <param name="manager"><see cref="IUserManager"/></param>
    /// <returns>Списко всех режиссёров</returns>
    public static async Task<IResult> GetAllDirectorsAsync(IDirectorManager manager)
    {
        var result = await manager.GetAllDirectorsAsync();

        return Results.Ok(result.Value);
    }
    
    /// <summary>
    ///     Обновление режиссёра
    /// </summary>
    /// <param name="request">Данные режиссера</param>
    /// <param name="manager"><see cref="IUserManager"/></param>
    /// <returns>Результат обновления</returns>
    public static async Task<IResult> UpdateDirectorByIdAsync(UpdateDirectorRequest request, IDirectorManager manager)
    {
        var result = await manager.UpdateDirectorByIdAsync(request);

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
    ///     Удаление режиссёра
    /// </summary>
    /// <param name="id">идентификатор удаляемого пользователя</param>
    /// <param name="manager"><see cref="IUserManager"/></param>
    /// <returns>Результат удаления</returns>
    public static async Task<IResult> DeleteDirectorByIdAsync(Guid id, IDirectorManager manager)
    {
        var result = await manager.DeleteDirectorByIdAsync(id);

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