using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы FavoriteMoviesUsers
/// </summary>
public class FavoriteUserMoviesConfiguration : IEntityTypeConfiguration<FavoriteMoviesUsers>
{
    public void Configure(EntityTypeBuilder<FavoriteMoviesUsers> builder)
    {
        builder.HasKey(fum => new { fum.MovieId, fum.UserId });

        builder.HasOne(fum => fum.Movie)
            .WithMany(m=>m.FavoriteUserMovies)
            .HasForeignKey(fum => fum.MovieId);

        builder.HasOne(fum => fum.User)
            .WithMany(u=>u.FavoriteUserMovies)
            .HasForeignKey(fum => fum.UserId);
    }
}