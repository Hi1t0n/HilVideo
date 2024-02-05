using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using AuthService.Domain.Interfaces;
using UserService.Domain.Contracts;
using UserService.Domain.Models;
using AuthService.Infrastructure.Helpers;
using UserService.Infrastructure.Context;
using CSharpFunctionalExtensions;
using UserService.Infrastructure.ErrorObjects;

namespace UserService.Infrastructure.Repositories;

public class UserManager : IUserManager
{
    private readonly ApplicationDbContext _context;

    private readonly IPasswordHasher _passwordHasher;
    
    public UserManager(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Result<List<GetAllUsersResponse>>> GetAllUserAsync()
    {
        var users = await _context.Users.Join(_context.Roles,
            u => u.RoleId,
            r => r.RoleId,
            (u, r) => new GetAllUsersResponse(u.UserId, u.Login, r.RoleName, u.Email, u.PhoneNumber, u.CreatedDate)
        ).ToListAsync();
        
        return Result.Success(users);
    }

    public async Task<Result<GetUserByIdResponse, IError>>? GetUserByIdAsync(Guid id)
    {
        var existingUser = await _context.Users.Join(_context.Roles,
            u => u.RoleId,
            r => r.RoleId,
            (u, r) => new
            {
                UserId = u.UserId,
                Login = u.Login,
                RoleName = r.RoleName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                CreateDate = u.CreatedDate
            }

        ).Where(u => u.UserId == id).FirstOrDefaultAsync();

        if (existingUser is null)
        {
            return Result.Failure<GetUserByIdResponse,IError>(new NotFoundError("Пользователь не найден"));
        }

        GetUserByIdResponse user = new GetUserByIdResponse(
            existingUser.UserId,
            existingUser.Login,
            existingUser.Email,
            existingUser.PhoneNumber,
            existingUser.RoleName,
            existingUser.CreateDate
            );
        return Result.Success<GetUserByIdResponse,IError>(user);
    }
    
    

    public async Task<Result<User,IError>>? UpdateUserByIdAsync(UpdateUserByIdRequest request)
    {
        if(string.IsNullOrWhiteSpace(request.login) || request.login.Length  > 30)
        {
            return Result.Failure<User,IError>(new BadRequestError("Логин не может быть пустым или больше 30 символов"));
        }
        
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.id);

        if (existingUser is null)
        {
            return Result.Failure<User, IError>(new NotFoundError("Пользователь не найден"));
        }

        existingUser.Login = request.login;
        existingUser.PhoneNumber = request.phoneNumber;
        existingUser.Email = request.email;

        _context.Update(existingUser);
        await _context.SaveChangesAsync();
        return Result.Success<User, IError>(existingUser);

    }

    public async Task<Result<User, IError>>? DeleteUserByIdAsync(Guid id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (existingUser is null)
        {
            return Result.Failure<User, IError>(new NotFoundError("Пользователь не найден"));
        }

        _context.Remove(existingUser);
        await _context.SaveChangesAsync();
        return Result.Success<User, IError>(existingUser);
    }

    public async Task<Result<User, IError>> ChangeUserPasswordByIdAsync(ChangeUserPasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.currentPassword) || string.IsNullOrWhiteSpace(request.newPassword))
        {
            return Result.Failure<User,IError>(new BadRequestError("Пароль не может состоять из пробелов или быть пустым"));
        }
        
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.id);
        if (existingUser is null)
        {
            return Result.Failure<User,IError>(new NotFoundError("Пользователь не найден"));
        }

        if (!_passwordHasher.Verify(request.currentPassword, existingUser.Password))
        {
            return Result.Failure<User,IError>(new BadRequestError("Неверный текущий пароль"));
        }

        existingUser.Password = _passwordHasher.HashPassword(request.newPassword);

        _context.Update(existingUser);
        await _context.SaveChangesAsync();
        return Result.Success<User,IError>(existingUser);
    }

    public async Task<Result<GetUserByLoginResponse, IError>> GetUserByLoginAsync(string login)
    {
        var existingUser = await _context.Users.Join(_context.Roles,
            u => u.RoleId,
            r => r.RoleId, (u, r) => new 
            {
                u.UserId,
                u.Login,
                r.RoleName,
                u.Email,
                u.PhoneNumber,
                u.CreatedDate
            }).Where(u => u.Login == login).FirstOrDefaultAsync();

        if (existingUser is null)
        {
            return Result.Failure<GetUserByLoginResponse, IError>(new NotFoundError("Пользователь не найден"));
        }

        GetUserByLoginResponse userResponse = new GetUserByLoginResponse(existingUser.UserId, existingUser.Login,
            existingUser.RoleName, existingUser.Email, existingUser.PhoneNumber, existingUser.CreatedDate);
        
        return Result.Success<GetUserByLoginResponse, IError>(userResponse);
    }
}