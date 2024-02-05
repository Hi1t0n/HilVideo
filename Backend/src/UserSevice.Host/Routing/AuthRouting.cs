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
        authGroup.MapGet(pattern: "/logout", handler: Logout);
        return application;
    }

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