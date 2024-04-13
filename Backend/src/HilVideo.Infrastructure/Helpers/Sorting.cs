using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.Enums;

namespace Infrastructure.Helpers;

public class Sorting : ISorting
{
    public IQueryable<Movie> ApplySorting(IQueryable<Movie> queryable, SortBy sortBy = SortBy.SortByName)
    {
        switch (sortBy)
        {
            case SortBy.SortByName:
                return queryable.OrderBy(m => m.MovieName);
            case SortBy.SortByReliseDate:
                return queryable.OrderBy(m => m.ReleaseDate);
            case SortBy.SortByReliseDateDesc:
                return queryable.OrderByDescending(m => m.ReleaseDate);
            default:
                return queryable;
        }
    }
}