namespace UserService.Domain.Contracts;

public record AddMovieRequest(string MovieName, string MovieDescription, Guid MovieType, List<Guid> Directors, List<Guid> Genres);