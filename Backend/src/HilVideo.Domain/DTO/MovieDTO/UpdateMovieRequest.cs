namespace UserService.Domain.Contracts;

public record UpdateMovieRequest
    (
        Guid MovieId,
        string MovieName,
        string MovieDescription,
        DateTime ReleaseDate,
        Guid MovieType,
        List<Guid>? RemovedDirectorsId,
        List<Guid>? AddedDirectorsId,
        List<Guid>? RemovedGenresId,
        List<Guid>? AddedGenresId
    );