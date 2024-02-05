namespace AuthService.Domain.Contracts;

public record UserRegisterResponse(string Login,string Role, string? Email, string? PhoneNumber, string CreateDate);