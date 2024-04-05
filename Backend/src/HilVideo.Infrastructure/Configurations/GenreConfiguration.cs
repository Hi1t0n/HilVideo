using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы Genre
/// </summary>
public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.GenreId);
        builder.Property(g => g.GenreId).IsRequired();
        builder.HasIndex(g => g.GenreId).IsUnique();
        builder.Property(g => g.GenreName).IsRequired();
        builder.HasIndex(g => g.GenreName).IsUnique();

        builder.HasMany(g => g.Movies)
            .WithMany(m => m.Genres)
            .UsingEntity<MovieGenre>(
                j => j.HasOne(mg => mg.Movie)
                    .WithMany()
                    .HasForeignKey(mg => mg.MovieId),
                j => j.HasOne(mg => mg.Genre)
                    .WithMany()
                    .HasForeignKey(mg => mg.GenreId),
                j => j.HasKey(mg => new { mg.GenreId, mg.MovieId })
            );
    }
}