namespace UserService.Domain.Contracts;

public record LoginUserResponse(Guid id,string Login,string Role, string? Email, string? PhoneNumber, string CreateDate);