namespace UserService.Domain.Contracts.DirectorDTO;

public record DirectorResponse(Guid Id, string FirstName, string SecondName, string? Patronymic);