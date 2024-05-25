namespace UserService.Domain.Contracts;

public record GetMoviesResponse(Guid Id, string MovieName, string MovieDescription,string PosterFilePath, string MovieType, DateTime ReleaseDate, List<string> Directors, List<string> Genres);