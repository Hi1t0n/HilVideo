namespace UserService.Domain.DTO.BookDTO;

public record CheckBookFromFavoritesRequest(Guid UserId, Guid BookId);