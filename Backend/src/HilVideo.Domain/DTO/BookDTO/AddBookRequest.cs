using Microsoft.AspNetCore.Http;

namespace UserService.Domain.DTO.BookDTO;

public record AddBookRequest(string BookName, string BookDescription, DateTime ReleaseDate, IFormFile? PosterFile, IFormFile? BookFile, List<Guid> AuthorsId, List<Guid> GenresId);