using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Enums;

namespace Infrastructure.Helpers;

public class Sorting : ISorting
{
    public IQueryable<Movie> ApplySorting(IQueryable<Movie> queryable, int? sortBy = 1)
    {
        switch (sortBy)
        {
            case 1:
                return queryable.OrderBy(m => m.MovieName);
            case 2:
                return queryable.OrderBy(m => m.ReleaseDate);
            case 3:
                return queryable.OrderByDescending(m => m.ReleaseDate);
            default:
                return queryable;
        }
    }

    public IQueryable<Book> ApplySorting(IQueryable<Book> queryable, int? sortBy = 1)
    {
        switch (sortBy)
        {
            case 1:
                return queryable.OrderBy(b => b.BookName);
            case 2:
                return queryable.OrderBy(b => b.ReleaseDate);
            case 3:
                return queryable.OrderByDescending(b => b.ReleaseDate);
            default:
                return queryable;
        }
    }
}