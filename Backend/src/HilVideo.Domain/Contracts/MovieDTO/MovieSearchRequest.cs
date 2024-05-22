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
            var parsedUrl = url.Split('?')[1];
            var paramsCollections = HttpUtility.ParseQueryString(parsedUrl);

            var movieName = paramsCollections["MovieName"];
            var genres = paramsCollections["Genres"]?.Split(',')
                .Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).Select(Guid.Parse).ToList();
            var sortBy = Convert.ToInt16(paramsCollections["SortBy"]);
            
            result = new MovieSearchRequest(movieName, genres, sortBy);
            return true;
        }
        catch
        {
            return false;
        }
    }
}