using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.ErrorObjects;

namespace UserSevice.Host.Routing;

//TODO: Переделать на switch 

public static class UserRouter
{
    public static WebApplication AddUserRouter(this WebApplication application)
    {
        var userGroup = application.MapGroup("/api/users");

        userGroup.MapGet(pattern: "/", handler: GetAllUserAsync).RequireAuthorization(policyNames: "OnlyOwnerPolicy");
        userGroup.MapGet(pattern: "/{id:guid}", handler: GetUserByIdAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        //userGroup.MapPost(pattern: "/", handler: CreateUserAsync);
        userGroup.MapPut(pattern: "/", handler: UpdateUserByIdAsync).RequireAuthorization("UserAdminOwnerPolicy");
        userGroup.MapPut(pattern: "/changepassword", handler: ChangeUserPasswordByIdAsync).RequireAuthorization("UserAdminOwnerPolicy");
        userGroup.MapDelete(pattern: "/{id:guid}", handler: DeleteUserByIdAsync).RequireAuthorization("UserAdminOwnerPolicy");
        userGroup.MapGet(pattern: "/{login}",handler: GetUserByLoginAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");

        return application;
    }
    
    /// <summary>
    /// Получение всех пользователей
    /// </summary>
    /// <param name="userManager"></param>
    /// <returns>Результат получения всех пользователей</returns>
    public static async Task<IResult> GetAllUserAsync(IUserManager userManager)
    {
        var users = await userManager.GetAllUserAsync();

        return Results.Ok(users.Value);
    }
    
    /// <summary>
    /// Получение пользователя по id
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя</param>
    /// <param name="userManager"></param>
    /// <returns>Результат получения пользователя</returns>
    public static async Task<IResult> GetUserByIdAsync(Guid id, IUserManager userManager)
    {
        var result = await userManager.GetUserByIdAsync(id);

        if (result.IsFailure)
        {
            if (result.Error is BadRequestError)
            {
                return Results.BadRequest(new
                {
                    error = result.Error.ErrorMessange
                });
            }

            if (result.Error is NotFoundError)
            {
                return Results.NotFound(new
                {
                    error = result.Error.ErrorMessange
                });
            }
        }

        return Results.Ok(result.Value);
    }
    
    /// <summary>
    /// Получение пользователя по логину
    /// </summary>
    /// <param name="login">Логин пользователя</param>
    /// <param name="userManager"></param>
    /// <returns>Результат получения пользователя</returns>
    public static async Task<IResult> GetUserByLoginAsync(string login, IUserManager userManager)
    {
        var result = await userManager.GetUserByLoginAsync(login);
        if (result is { IsFailure: true, Error: NotFoundError })
        {
            return Results.NotFound(new
            {
                error = result.Error.ErrorMessange
            });
        }

        return Results.Ok(result.Value);

    }
    
    /// <summary>
    /// Обновление пользователя по id
    /// </summary>
    /// <param name="request">DTO с данными пользователя</param>
    /// <param name="userManager"></param>
    /// <returns>Результат обновления данных</returns>
    public static async Task<IResult> UpdateUserByIdAsync(UpdateUserByIdRequest request, IUserManager userManager)
    {
        var result = await userManager.UpdateUserByIdAsync(request);
        
        if (result.IsFailure)
        {
            if (result.Error is BadRequestError)
            {
                return Results.BadRequest(new
                {
                    error = result.Error.ErrorMessange
                });
            }

            if (result.Error is NotFoundError)
            {
                return Results.NotFound(new
                {
                    error = result.Error.ErrorMessange
                });
            }
        }

        return Results.Ok(result.Value);
    }
    
    /// <summary>
    /// Смена пароля пользователя по id
    /// </summary>
    /// <param name="request">DTO с данными</param>
    /// <param name="userManager"></param>
    /// <returns>Результат смены пароля</returns>
    public static async Task<IResult> ChangeUserPasswordByIdAsync(ChangeUserPasswordRequest request, IUserManager userManager)
    {
        var result = await userManager.ChangeUserPasswordByIdAsync(request);

        if (result.IsFailure)
        {
            if (result.Error is NotFoundError)
            {
                return Results.NotFound(new
                {
                    error = result.Error.ErrorMessange
                });
            }

            if (result.Error is BadRequestError)
            {
                return Results.BadRequest(new
                {
                    error = result.Error.ErrorMessange
                });
            }
        }

        return Results.Ok(result.Value);
    }
    
    /// <summary>
    /// Удаление пользователя по id
    /// </summary>
    /// <param name="id">Уникальный идентификатор пользователя</param>
    /// <param name="userManager"></param>
    /// <returns>Результат удаления пользователя</returns>
    public static async Task<IResult> DeleteUserByIdAsync(Guid id, IUserManager userManager)
    {
        var result = await userManager.DeleteUserByIdAsync(id);

        if (result.IsFailure)
        {
            if (result.Error is NotFoundError)
            {
                return Results.NotFound(new
                {
                    error = result.Error.ErrorMessange
                });
            }

            if (result.Error is BadRequestError)
            {
                return Results.BadRequest(new
                {
                    error = result.Error.ErrorMessange
                });
            }
        }

        return Results.Ok(result.Value);
    }
}