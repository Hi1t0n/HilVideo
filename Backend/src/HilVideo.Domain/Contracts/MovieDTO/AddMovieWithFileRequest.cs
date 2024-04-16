using Microsoft.AspNetCore.Http;

namespace UserService.Domain.Contracts;

public record AddMovieWithFileRequest(string MovieName, string MovieDescription, Guid MovieType,DateTime ReleaseData,IFormFile? PosterFile, IFormFile? MovieFile, List<Guid> Directors, List<Guid> Genres);
