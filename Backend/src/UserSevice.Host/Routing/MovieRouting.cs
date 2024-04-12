namespace UserSevice.Host.Routing;

public static class MovieRouting
{
    public static WebApplication AddMovieRouting(this WebApplication application)
    {
        var movieGroup = application.MapGroup("/api/movies");

        return application;
    }
}