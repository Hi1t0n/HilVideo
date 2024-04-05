namespace UserService.Domain.Models;

public class Genre
{
    public Guid GenreId { get; set; } /* id  */
    public string GenreName { get; set; } /* Название жанра */
    public List<Movie> Movies { get; set; } = new(); /* Связь с таблицей Movie многие ко многим */
    public List<MovieGenre> MovieGenres { get; set; } = new(); /* Связь с таблицей MovieGenre многие ко многим */
}