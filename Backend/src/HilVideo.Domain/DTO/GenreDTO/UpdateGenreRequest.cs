namespace UserService.Domain.Contracts.GenreDTO;

public record UpdateGenreRequest(Guid Id, string? NewName);