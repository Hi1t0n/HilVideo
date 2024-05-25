namespace UserService.Domain.Models;

public class Book
{
    public Guid BookId { get; set; }
    public required string BookName { get; set; }
    public required string BookDescription { get; set; }
    public required string PosterFilePath { get; set; }
    public required string BookFilePath { get; set; }
    public required DateTime ReleaseDate { get; set; }
    public List<User> Users { get; set; } = new();
    public List<Author> Authors { get; set; } = new();
    public List<BookAuthor> BookAuthors { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();
    public List<BookGenre> BookGenres { get; set; } = new();
    public List<FavoriteBooksUsers> FavoriteBooksUsers { get; set; } = new();
}