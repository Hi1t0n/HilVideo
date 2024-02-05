namespace UserService.Domain.Contracts;

public record LoginUserResponse(string Login,string Role, string? Email, string? PhoneNumber, string CreateDate);