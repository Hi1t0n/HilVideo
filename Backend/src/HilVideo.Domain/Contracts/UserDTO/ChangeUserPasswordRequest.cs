namespace UserService.Domain.Contracts;

public record ChangeUserPasswordRequest(Guid id, string? currentPassword, string? newPassword);