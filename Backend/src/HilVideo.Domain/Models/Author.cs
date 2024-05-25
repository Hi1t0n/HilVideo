namespace UserService.Domain.Models;

public class Author
{
    public required Guid AuthorId { get; set; }
    public required string FirstName { get; set; }
    public required string SecondName { get; set; }
    public string? Patronymic { get; set; }
    public List<Book> Books { get; set; } = new();
    public List<BookAuthor> BookAuthors { get; set; } = new();
}