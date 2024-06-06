namespace UserService.Domain.DTO.MovieDTO;

public record CheckMovieFromFavoritesRequest(Guid UserId, Guid MovieId);