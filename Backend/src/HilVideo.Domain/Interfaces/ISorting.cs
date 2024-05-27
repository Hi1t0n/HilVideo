using UserService.Domain.Models;
using UserService.Infrastructure.Enums;

namespace UserService.Domain.Interfaces;

public interface ISorting
{
    IQueryable<Movie> ApplySorting(IQueryable<Movie> queryable, int? sortBy = 1);
    IQueryable<Book> ApplySorting(IQueryable<Book> queryable, int? sortBy = 1);
}