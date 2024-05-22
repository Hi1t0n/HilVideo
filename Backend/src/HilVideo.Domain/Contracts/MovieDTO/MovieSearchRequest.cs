using System.Web;

namespace UserService.Domain.Contracts;

public record MovieSearchRequest(string MovieName, List<Guid>? Genres, int SortBy)
{
    public static bool TryParse(string url, out MovieSearchRequest result)
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
            var movieName = paramsCollections["MovieName"];
            var genres = paramsCollections["Genres"]?.Split(',')
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(Guid.Parse).ToList();
            var sortBy = paramsCollections["SortBy"];
            
            int? sortByInt = !string.IsNullOrEmpty(sortBy) ? Convert.ToInt32(sortBy) : null;
        
            // Создаем объект
            result = new MovieSearchRequest(movieName, genres, sortByInt ?? default);
            return true;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

}