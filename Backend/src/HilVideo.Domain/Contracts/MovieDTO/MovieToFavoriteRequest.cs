namespace UserService.Domain.Contracts;

public record MovieToFavoriteRequest(Guid userId, Guid movieId);