namespace UserService.Domain.Models;

/* Таблица MovieFiles для хранения путей файлов от фильмов*/
public class MovieFile
{
    public Guid MovieFileId { get; set; } /* id */
    public Guid MovieId { get; set; } /* id фильма которому принадлежит файл */
    public string FilePath { get; set; } /* Путь к файлу на сервере */
    public int? EpisodNumber { get; set; /* Номер эпизода */ }
    public Movie Movie { get; set; } /* Свойство для связи с таблицей Movie*/
}