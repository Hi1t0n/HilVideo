namespace UserService.Domain.Contracts.MovieTypeDTO;

public record MovieTypeUpdateRequest(Guid Id, string NewMovieTypeName);