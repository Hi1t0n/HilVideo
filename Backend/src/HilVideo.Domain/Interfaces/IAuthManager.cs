using AuthService.Domain.Contracts;
using CSharpFunctionalExtensions;
using UserService.Domain.DTO.AuthDTO;
using UserService.Domain.Interfaces;

namespace AuthService.Domain.Interfaces;

public interface IAuthManager
{
    public Task<Result<UserRegisterResponse, IError>> Register(UserRegisterRequest request);
    public Task<Result<LoginDataResponse, IError>> Login(LoginUserRequest request);
}