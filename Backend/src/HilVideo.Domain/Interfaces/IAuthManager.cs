using AuthService.Domain.Contracts;
using CSharpFunctionalExtensions;
using UserService.Domain.Interfaces;

namespace AuthService.Domain.Interfaces;

public interface IAuthManager
{
    public Task<Result<UserRegisterResponse, IError>> Register(UserRegisterRequest request);
    public Task<Result<string, IError>> Login(LoginUserRequest request);
}