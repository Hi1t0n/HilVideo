using System.Security.Claims;
using System.Text;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.Repositories;
using AuthService.Infrastructure;
using Infrastructure.Helpers;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection serviceCollection,
        IConfiguration configuration, string connectionString)
    {
        serviceCollection.AddManager();
        serviceCollection.AddDatabase(connectionString);
        serviceCollection.AddApiAuthentication(configuration);
        return serviceCollection;
    }
    
    private static IServiceCollection AddManager(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserManager, UserManager>();
        serviceCollection.AddScoped<IJwtProvider, JwtProvider>();
        serviceCollection.AddScoped<ICheckUserData, CheckUserData>();
        serviceCollection.AddScoped<IAuthManager, AuthManager>();
        serviceCollection.AddScoped<IPasswordHasher, PasswordHasher>();
        serviceCollection.AddScoped<IFileHelper, FileHelper>();
        serviceCollection.AddScoped<IWordsTranslate, WordsTranslate>();
        serviceCollection.AddScoped<ISorting, Sorting>();
        serviceCollection.AddScoped<IMovieManager, MovieManager>();
        serviceCollection.AddScoped<IGenreManager, GenreManager>();
        serviceCollection.AddScoped<IDirectorManager, DirectorManager>();
        serviceCollection.AddScoped<IMovieTypeManager, MovieTypeManager>();
        serviceCollection.AddScoped<IAuthorManager, AuthorManager>();
        serviceCollection.AddScoped<IBookManager, BookManager>();
        
        return serviceCollection;
    }
    
    private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(builder => builder.UseNpgsql(connectionString));
        return serviceCollection;
    }

    public static void AddApiAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {

        var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions!.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["token"];
                        
                        return Task.CompletedTask;
                    }
                };
            });
        serviceCollection.AddAuthorization(options =>
        {
            options.AddPolicy("OnlyOwnerPolicy", policy =>
            {
                policy.RequireClaim("Role", "Owner"); 
            });
            options.AddPolicy("AdminOwnerPolicy", policy =>
            { 
                policy.RequireClaim("Role", "Owner", "Admin");
            });
            options.AddPolicy("UserAdminOwnerPolicy", policy =>
            {
                policy.RequireClaim("Role", "User", "Admin", "Owner");
            });
        });
    }
}