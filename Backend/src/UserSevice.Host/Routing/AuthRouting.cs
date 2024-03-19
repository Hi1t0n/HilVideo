using AuthService.Domain.Contracts;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;
using UserService.Infrastructure.Repositories;

namespace UserSevice.Host.Routing;

public static class AuthRouting
{
    public static WebApplication AddAuthRouting(this WebApplication application)
    {
        var authGroup = application.MapGroup("/api/auth");
        authGroup.MapPost(pattern: "/register",handler: Register);
        authGroup.MapPost(pattern: "/login", handler: Login);
        authGroup.MapGet(pattern: "/logout", handler: Logout).RequireAuthorization();
        return application;
    }
    
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="request">DTO с данными пользователя</param>
    /// <param name="authManager">Объекта authManager с методами для работы с бд</param>
    /// <returns>Результат регистрации пользователя</returns>
    public static async Task<IResult> Register(UserRegisterRequest request, IAuthManager authManager)
    {
        var result = await authManager.Register(request);

        if (result.IsFailure)
        {
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
    /// Логин пользователя
    /// </summary>
    /// <param name="request">DTO с данными пользователя для входа</param>
    /// <param name="authManager"></param>
    /// <param name="userManager"></param>
    /// <param name="httpContext"></param>
    /// <returns>Результат входа пользователя</returns>
    public static async Task<IResult> Login(LoginUserRequest request, IAuthManager authManager,IUserManager userManager, HttpContext httpContext)
    {
        var result = await authManager.Login(request);

        if (result.IsFailure)
        {
            if (result.Error is BadRequestError)
            {
                return Results.BadRequest(result.Error.ErrorMessange);
            }
            
            if (result.Error is UnauthorizedError)
            {
                return Results.Unauthorized();
            }
        }
        
        httpContext.Response.Cookies.Append("token",result.Value);

        var user = await userManager.GetUserByLoginAsync(request.Login);
        
        return Results.Ok(user.Value);
    }
    
    //TODO: async
    /// <summary>
    /// Выход пользователя из системы
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Результат выхода пользователя</returns>
    public static async Task<IResult> Logout(HttpContext context)
    {
        if (!context.Request.Cookies.ContainsKey("token"))
        {
            return Results.Unauthorized();
        }
        
        context.Response.Cookies.Delete("token");
        return Results.Ok();
    }
}