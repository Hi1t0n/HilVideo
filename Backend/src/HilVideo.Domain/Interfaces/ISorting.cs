using UserService.Domain.Models;
using UserService.Infrastructure.Enums;

namespace UserService.Domain.Interfaces;

public interface ISorting
{
    IQueryable<Movie> ApplySorting(IQueryable<Movie> queryable, SortBy sortBy = SortBy.SortByName);
}