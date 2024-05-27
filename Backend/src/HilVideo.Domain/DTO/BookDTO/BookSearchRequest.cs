using System.Web;

namespace UserService.Domain.DTO.BookDTO;

public record BookSearchRequest(string? BookName, List<Guid>? Genres, int? SortBy)
{
    public static bool TryParse(string url, out BookSearchRequest? result)
    {
        result = null;
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }
    
        try
        {
            var parsedUrl = url.Split('?');
            var paramsCollections = HttpUtility.ParseQueryString(parsedUrl.Length > 1 ? parsedUrl[1] : "");

            // Извлекаем значения параметров из строки 
            var bookName = paramsCollections["BookName"];
            var genres = paramsCollections["Genres"]?.Split(',')
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(Guid.Parse).ToList();
            var sortBy = paramsCollections["SortBy"];
            
            int? sortByInt = !string.IsNullOrEmpty(sortBy) ? Convert.ToInt32(sortBy) : null;
        
            // Создаем объект
            result = new BookSearchRequest(bookName, genres, sortByInt ?? default);
            return true;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

}