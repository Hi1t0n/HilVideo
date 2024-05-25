namespace UserService.Domain.DTO.AuthorDTO;

public record UpdateAuthorRequest(Guid Id, string FirstName, string SecondName, string? Patronymic);