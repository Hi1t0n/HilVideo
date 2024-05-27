namespace UserService.Domain.DTO.BookDTO;

public record GetBookByIdResponse(Guid BookId, string BookName, string BookDescription , string BookFilePath,string PosterFilePath, DateTime ReleaseDate, List<string> Authors, List<string> Genres);