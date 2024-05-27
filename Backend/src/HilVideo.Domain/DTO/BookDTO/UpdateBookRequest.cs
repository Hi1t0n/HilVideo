namespace UserService.Domain.DTO.BookDTO;

public record UpdateBookRequest(
    Guid BookId,
    string BookName,
    string BookDescription,
    DateTime ReleaseDate,
    List<Guid>? RemovedAuthorsId,
    List<Guid>? AddedAuthorsId,
    List<Guid>? RemovedGenresId,
    List<Guid>? AddedGenresId
);