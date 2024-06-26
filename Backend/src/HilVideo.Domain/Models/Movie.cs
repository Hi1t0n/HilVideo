namespace UserService.Domain.Models;

public class Movie
{
    public Guid MovieId { get; set; } /* id */
    public string MovieName { get; set; } /* Название фильма */
    public string MovieDescription { get; set; } /* Описание фильма */
    public string PosterFilePath { get; set; } /* Постер фильма */
    public Guid MovieTypeId { get; set; } /* id типа фильма */
    public DateTime ReleaseDate { get; set; } /* Дата релиза */
    public MovieType MovieType { get; set; } /* Связь с таблицей MovieType */
    public List<Director> Directors { get; set; } = new(); /* Связь с таблицей Director многие ко многим */
    public List<MovieDirector> MovieDirectors { get; set; } = new(); /* Связь с промежуточной таблицей MovieDirector */
    public List<Genre> Genres { get; set; } = new(); /* Связь с таблицей Genre многие ко многим */
    public List<MovieGenre> MovieGenres { get; set; } = new(); /* Связь с промежуточной таблицей MovieGenre */
    public List<User> Users { get; set; } = new(); /* Связь с таблицей User многие ко многим */
    public List<FavoriteMoviesUsers> FavoriteMoviesUsers { get; set; }= new(); /* Связь с таблицей FavoriteMoviesUsers многие ко многим */
    public List<MovieFile> MovieFiles { get; set; } = new(); /* Связь один ко многи с таблицей MovieFile */
}