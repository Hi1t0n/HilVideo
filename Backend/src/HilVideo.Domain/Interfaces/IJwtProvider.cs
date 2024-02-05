using AuthService.Domain.Contracts;

namespace UserService.Domain.Interfaces;

public interface IJwtProvider
{
    public string GenerateToken(UserData user);
}