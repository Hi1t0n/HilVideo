namespace UserService.Domain.Contracts;

public record GetAllUsersResponse(Guid UserId, string Login, string RoleName, string? Email, string? PhoneNumber, string CreateDate);