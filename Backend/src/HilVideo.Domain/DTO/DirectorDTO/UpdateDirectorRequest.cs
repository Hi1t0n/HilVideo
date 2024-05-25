namespace UserService.Domain.Contracts.DirectorDTO;

public record UpdateDirectorRequest(Guid Id, string FirstName, string SecondName, string? Patronymic);