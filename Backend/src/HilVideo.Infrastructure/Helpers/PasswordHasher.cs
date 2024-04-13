using AuthService.Domain.Interfaces;

namespace Infrastructure.Helpers;

public class PasswordHasher : IPasswordHasher
{
    /// <summary>
    /// Хеширование пароля
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <returns>Защифрованный пароль</returns>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }
    
    /// <summary>
    /// Проверка пароля
    /// </summary>
    /// <param name="password">Введеный пароль</param>
    /// <param name="hashedPassword">Хэш пароля из бд</param>
    /// <returns>true - совпадают, false - не свопадают </returns>
    public bool Verify(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}