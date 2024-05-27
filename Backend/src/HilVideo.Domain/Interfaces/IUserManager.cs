using UserService.Domain.Models;
using UserService.Domain.Contracts;
using CSharpFunctionalExtensions;

namespace UserService.Domain.Interfaces;

public interface IUserManager 
{
    Task<Result<List<GetAllUsersResponse>>>  GetAllUserAsync();

    Task<Result<GetUserByIdResponse,IError>> GetUserByIdAsync(Guid id);

    //Task<Result<User, IError>>? CreateUserAsync(UserRegisterRequest request);

    Task<Result<User,IError>> UpdateUserByIdAsync(UpdateUserByIdRequest request);

    Task<Result<User,IError>> DeleteUserByIdAsync(Guid id);
    Task<Result<User, IError>> ChangeUserPasswordByIdAsync(ChangeUserPasswordRequest request);
    Task<Result<GetUserByLoginResponse, IError>> GetUserByLoginAsync(string login);

    Task<Result<User, IError>> MakeUserAnAdminByLogin(string login);
    Task<Result<User, IError>> RemoveUserAnAdminByLogin(string login);
}