using UserSevice.Host.Routing;

namespace UserSevice.Host.Extensions;

public static class RoutingExtensions
{
    public static WebApplication AddRouting(this WebApplication application)
    {
        application.AddUserRouter();
        application.AddAuthRouting();
        application.AddMovieRouting(); 
        application.AddGenreRouting();
        application.AddDirectorRouting();
        application.AddMovieTypeRouting();
        application.AddAuthorRouting();
        application.AddBookRouting();
        
        return application;
    }
}