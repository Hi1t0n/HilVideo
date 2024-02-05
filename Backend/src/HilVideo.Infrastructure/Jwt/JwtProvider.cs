

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Domain.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace AuthService.Infrastructure;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _options;
    
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateToken(UserData user)
    {
        Claim[] claims =
            [new("UserId", user.UserId.ToString()), new("Role", user.RoleName), new("CreatedDate", user.CreatedDate)];
        
        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
        claims: claims,
        signingCredentials: signingCredentials,
        expires: DateTime.UtcNow.AddHours(_options.ExpitesHours),
        audience: _options.Audience,
        issuer: _options.Issuer);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}