namespace UserService.Domain.Models;

public class FavoriteMoviesUsers
{
    public Guid UserId { get; set; } /* id пользователя */
    public User? User { get; set; } /* Связь с таблицей User */
    public Guid MovieId { get; set; } /* id фильма */
    public Movie? Movie { get; set; } /* Связь с таблицей Movie */
    
}