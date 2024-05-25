namespace UserService.Domain.Contracts;

public record UpdateUserByIdRequest(Guid id, string login, string email, string phoneNumber);