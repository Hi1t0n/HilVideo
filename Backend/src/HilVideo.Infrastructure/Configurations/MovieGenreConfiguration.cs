using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы MovieGenre
/// </summary>
public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.HasKey(mg => new { mg.MovieId, mg.GenreId });

        builder.HasOne(mg => mg.Movie)
            .WithMany(m=>m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);

        builder.HasOne(mg => mg.Genre)
            .WithMany(g=> g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);
    }
}