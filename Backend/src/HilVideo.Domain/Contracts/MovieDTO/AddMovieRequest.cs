namespace UserService.Domain.Contracts;

public record AddMovieRequest(string MovieName, string MovieDescription, Guid MovieType,DateTime ReleaseDate, List<Guid> Directors, List<Guid> Genres);