namespace UserService.Domain.Models;

/// <summary>
/// Промежутоная таблица для связи многие ко многим между таблицами Movie и Director
/// </summary>
public class MovieDirector
{
    public Guid MovieId { get; set; } /* id Фильма*/
    public Movie? Movie { get; set; } /* Связь с таблицей Movie */
    public Guid DirectorId { get; set; } /* id Режиссера */
    public Director? Director { get; set; } /* Связь с таблицей Director */
}