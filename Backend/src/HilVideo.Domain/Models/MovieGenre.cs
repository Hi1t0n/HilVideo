namespace UserService.Domain.Models;

public class MovieGenre
{
    public Guid MovieId { get; set; } /* id */
    public Movie? Movie { get; set; } /* Связь с таблицей Movie */
    public Guid GenreId { get; set; } /* id */
    public Genre? Genre { get; set; } /* Связь с таблицей Genre */
}