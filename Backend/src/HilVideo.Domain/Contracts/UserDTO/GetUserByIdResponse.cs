namespace UserService.Domain.Contracts;

public record GetUserByIdResponse(Guid Id, string Login, string? Email, string? PhoneNumber, string RoleName, string CreateDate);