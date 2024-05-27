namespace UserService.Domain.DTO.BookDTO;

public record BookToFavoriteRequest(Guid UserId, Guid BookId);