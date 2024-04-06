using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы FavoriteMoviesUsers
/// </summary>
public class FavoriteMoviesUserConfiguration : IEntityTypeConfiguration<FavoriteMoviesUsers>
{
    public void Configure(EntityTypeBuilder<FavoriteMoviesUsers> builder)
    {
        builder.HasKey(fum => new { fum.MovieId, fum.UserId });

        builder.HasOne(fum => fum.Movie)
            .WithMany(m=>m.FavoriteMoviesUsers)
            .HasForeignKey(fum => fum.MovieId);

        builder.HasOne(fum => fum.User)
            .WithMany(u=>u.FavoriteMoviesUsers)
            .HasForeignKey(fum => fum.UserId);
    }
}