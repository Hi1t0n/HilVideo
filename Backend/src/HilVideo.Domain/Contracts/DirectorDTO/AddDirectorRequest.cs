namespace UserService.Domain.Contracts.DirectorDTO;

public record AddDirectorRequest(string FirstName, string SecondName, string? Patronymic);