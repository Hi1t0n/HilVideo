namespace UserService.Domain.Contracts;

public record AddMovieRequest(string MovieName, string MovieDescription, Guid MovieType,DateTime ReliseDate, List<Guid> Directors, List<Guid> Genres);