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

        userGroup.MapGet(pattern: "/", handler: GetAllUserAsync).RequireAuthorization();
        userGroup.MapGet(pattern: "/{id:guid}", handler: GetUserByIdAsync);
        //userGroup.MapPost(pattern: "/", handler: CreateUserAsync);
        userGroup.MapPut(pattern: "/", handler: UpdateUserByIdAsync);
        userGroup.MapPut(pattern: "/changepassword", handler: ChangeUserPasswordByIdAsync);
        userGroup.MapDelete(pattern: "/{id:guid}", handler: DeleteUserByIdAsync);
        userGroup.MapGet(pattern: "/{login}",handler: GetUserByLoginAsync);

        return application;
    }

    public static async Task<IResult> GetAllUserAsync(IUserManager userManager)
    {
        var users = await userManager.GetAllUserAsync();

        return Results.Ok(users.Value);
    }

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