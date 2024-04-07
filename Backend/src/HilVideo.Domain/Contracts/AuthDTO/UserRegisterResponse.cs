namespace AuthService.Domain.Contracts;

public record UserRegisterResponse(string Login, string? Email, string? PhoneNumber, string CreateDate);