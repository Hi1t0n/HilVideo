using Microsoft.AspNetCore.Http;

namespace UserService.Domain.DTO.MovieDTO;

public record AddMovieRequest(string MovieName, string MovieDescription, Guid MovieType,DateTime ReleaseData,IFormFile? PosterFile, IFormFile? MovieFile, List<Guid> Directors, List<Guid> Genres);
