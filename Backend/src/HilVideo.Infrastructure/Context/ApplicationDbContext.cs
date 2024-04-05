using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using UserService.Infrastructure.Configurations;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {
        
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Director> Directors => Set<Director>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<MovieDirector> MoviesDirectors => Set<MovieDirector>();
    public DbSet<MovieGenre> MoviesGenres => Set<MovieGenre>();
    public DbSet<MovieType> MovieTypes => Set<MovieType>();
    public DbSet<FavoriteMoviesUsers> FavoriteMoviesUsers => Set<FavoriteMoviesUsers>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new DirectorConfiguration());
        modelBuilder.ApplyConfiguration(new GenreConfiguration());
        modelBuilder.ApplyConfiguration(new MovieConfiguration());
        modelBuilder.ApplyConfiguration(new MovieDirectorConfiguration());
        modelBuilder.ApplyConfiguration(new MovieGenreConfiguration());
        modelBuilder.ApplyConfiguration(new MovieTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FavoriteUserMoviesConfiguration());

    }
}