namespace UserService.Domain.DTO.BookDTO;

public record GetBooksResponse(Guid Id, string BookName, string BookDescription,string PosterFilePath, DateTime ReleaseDate, List<string> Authors, List<string> Genres);