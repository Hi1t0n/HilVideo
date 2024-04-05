namespace UserService.Domain.Models;

public class MovieType
{
    public Guid MovieTypeId { get; set; } /* id */
    public string MovieTypeName { get; set; } /* Название типа */
    public List<Movie> Movies { get; set; } = new (); /* Связь один ко многим с Movie */
}