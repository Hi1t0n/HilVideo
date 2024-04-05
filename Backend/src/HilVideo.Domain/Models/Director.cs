namespace UserService.Domain.Models;

public class Director
{
    public Guid DirectorId { get; set; } /* id */
    public string FirstName { get; set; } /* Имя режиссера */
    public string SecondName { get; set; } /* Фамилия режиссера */
    public string? Patronymic { get; set; } = string.Empty; /* Отчество режиссера */
    public List<Movie> Movies { get; set; } = new(); /* Связь с таблицей Movie многие ко многим */
    public List<MovieDirector> MovieDirectors { get; set; } = new(); /* Связь с промежуточной таблицей MovieDirector */

}