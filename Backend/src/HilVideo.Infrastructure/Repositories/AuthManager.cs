using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using UserService.Domain.Contracts;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Context;
using UserService.Infrastructure.ErrorObjects;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Repositories;

public class AuthManager : IAuthManager
{

    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICheckUserData _checkUserData;
    private readonly IJwtProvider _jwtProvider;
    
    public AuthManager(ApplicationDbContext context, IPasswordHasher passwordHasher, ICheckUserData checkUserData, IJwtProvider jwtProvider)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _checkUserData = checkUserData;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result<UserRegisterResponse, IError>> Register(UserRegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) || request.Login.Length > 30)
        {
            return Result.Failure<UserRegisterResponse, IError>(new BadRequestError("Логин не может быть пустым или больше 30 символов"));
        }

        if (_checkUserData.CheckUserLogin(request.Login).Result)
        {
            return Result.Failure<UserRegisterResponse, IError>(new BadRequestError("Логин уже занят"));
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length > 30)
        {
            return Result.Failure<UserRegisterResponse, IError>(new BadRequestError("Пароль не может быть пустым или больше 30 символо"));
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            if (_checkUserData.CheckUserEmail(request.Email).Result)
            {
                return Result.Failure<UserRegisterResponse, IError>(new BadRequestError("Почта уже используется"));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            if (_checkUserData.CheckUserPhoneNumber(request.PhoneNumber).Result)
            {
                return Result.Failure<UserRegisterResponse, IError>(new BadRequestError("Номер телефона уже используется"));
            }
        }

        Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "User");

        var existingUser = new User
        {
            UserId = Guid.NewGuid(),
            Login = request.Login,
            Password = _passwordHasher.HashPassword(request.Password),
            RoleId = role.RoleId,
            Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email,
            PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber,
            CreatedDate = DateTime.Now.ToShortDateString(),
        };

        await _context.Users.AddAsync(existingUser);
        await _context.SaveChangesAsync();
        return Result.Success<UserRegisterResponse, IError>(new UserRegisterResponse(existingUser.Login,role.RoleName,existingUser.Email,existingUser.PhoneNumber,existingUser.CreatedDate));
    }

    public async Task<Result<string,IError>> Login(LoginUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return Result.Failure<string, IError>(new BadRequestError("Пожалуйтса введите данные"));
        }

        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return Result.Failure<string, IError>(new BadRequestError("Пожалуйтса введите данные"));
        }
        
        var user = await _context.Users
            .Join(_context.Roles,
                u => u.RoleId,
                r => r.RoleId,
                (u, r) => new { User = u, RoleName = r.RoleName })
            .Where(joined => joined.User.Login == request.Login)
            .FirstOrDefaultAsync();

        if (user is null)
        {
            return Result.Failure<string,IError>(new UnauthorizedError("Неверный логин или пароль"));
        }

        var result = _passwordHasher.Verify(request.Password, user.User.Password);

        if (result is false)
        {
            return Result.Failure<string, IError>(new UnauthorizedError("Неверынй логин или пароль"));
        }

        var userData = new UserData(
            user.User.UserId,
            user.User.Login,
            user.RoleName,
            user.User.PhoneNumber,
            user.User.Email,
            user.User.CreatedDate
        );
        
        var token = _jwtProvider.GenerateToken(userData);
        
        
        return Result.Success<string,IError>(token);
    }

    
}