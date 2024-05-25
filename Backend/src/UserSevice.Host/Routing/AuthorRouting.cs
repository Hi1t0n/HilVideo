using UserService.Domain.DTO.AuthorDTO;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.ErrorObjects;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace UserSevice.Host.Routing;

public static class AuthorRouting
{
    public static WebApplication AddAuthorRouting(this WebApplication application)
    {
        var authorGroup = application.MapGroup("/api/author");

        authorGroup.MapPost(pattern: "/", handler: AddAuthorAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        authorGroup.MapGet(pattern: "/", handler: GetAllAuthorAsync);
        authorGroup.MapPut(pattern: "/", handler: UpdateAuthorByIdAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        authorGroup.MapDelete(pattern: "/{id:guid}", handler: DeleteAuthorByIdAsync).RequireAuthorization(policyNames: "AdminOwnerPolicy");
        
        return application;
    }

    public static async Task<IResult> AddAuthorAsync(AddAuthorRequest request, IAuthorManager manager)
    {
        var result = await manager.AddAuthorAsync(request);

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

    public static async Task<IResult> GetAllAuthorAsync(IAuthorManager manager)
    {
        var result = await manager.GetAllAuthorAsync();

        return Results.Ok();
    }

    public static async Task<IResult> UpdateAuthorByIdAsync(UpdateAuthorRequest request, IAuthorManager manager)
    {
        var result = await manager.UpdateAuthorByIdAsync(request);

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

    public static async Task<IResult> DeleteAuthorByIdAsync(Guid id, IAuthorManager manager)
    {
        var result = await manager.DeleteAuthorByIdAsync(id);
        
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
}