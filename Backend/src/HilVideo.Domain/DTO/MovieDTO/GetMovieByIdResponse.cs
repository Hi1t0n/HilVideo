namespace UserService.Domain.Contracts;

public record GetMovieByIdResponse(Guid MovieId, string MovieName, string MovieDescription , List<MovieFileDTO> MoviesFile,string PosterFilePath, string MovieType, DateTime ReleaseDate, List<string> Directors, List<string> Genres);