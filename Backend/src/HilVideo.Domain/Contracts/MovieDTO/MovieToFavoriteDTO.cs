namespace UserService.Domain.Contracts;

public record MovieToFavoriteDTO(Guid userId, Guid movieId);