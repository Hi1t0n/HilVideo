using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы Movie
/// </summary>
public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.MovieId);
        builder.Property(m => m.MovieId).IsRequired();
        builder.HasIndex(m => m.MovieId).IsUnique();
        builder.Property(m => m.MovieName).IsRequired();
        builder.Property(m => m.MovieDescription).IsRequired();
        
        /* Связь многие ко многим Movie и Director (MovieDirector) */
        builder.HasMany(m => m.Directors)
            .WithMany(d => d.Movies)
            .UsingEntity<MovieDirector>(
                j => j.HasOne(md => md.Director)
                    .WithMany()
                    .HasForeignKey(md => md.DirectorId),
                j => j.HasOne(md => md.Movie)
                    .WithMany()
                    .HasForeignKey(md => md.MovieId),
                j => j.HasKey(md => new { md.MovieId, md.DirectorId })
            );
        
        /* Связь многие ко многим Movie и Genre (movieGenre) */
        builder.HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity<MovieGenre>(
                j => j.HasOne(mg => mg.Genre)
                    .WithMany()
                    .HasForeignKey(mg => mg.GenreId),
                j => j.HasOne(mg => mg.Movie)
                    .WithMany()
                    .HasForeignKey(mg => mg.MovieId),
                j => j.HasKey(mg => new { mg.MovieId, mg.GenreId })
            );
        /* Связь многие ко многим Movie и User (FavoriteMoviesUsers) */
        builder.HasMany(m => m.Users)
            .WithMany(u => u.Movies)
            .UsingEntity<FavoriteMoviesUsers>(
                j => j.HasOne(fum => fum.User)
                    .WithMany()
                    .HasForeignKey(fum => fum.UserId),
                j => j.HasOne(fum => fum.Movie)
                    .WithMany()
                    .HasForeignKey(fum => fum.MovieId),
                j => j.HasKey(fum => new { fum.MovieId, fum.UserId })
            );
        
        
        /* Связь один к одному таблиц Movie и MovieType */
        builder.HasOne(m => m.MovieType)
            .WithMany(mt => mt.Movies)
            .HasForeignKey(m => m.MovieTypeId);
        
        /* Связь один ко многим таблиц Movie и MovieFiles */
        builder.HasMany(m => m.MovieFiles)
            .WithOne(mf => mf.Movie)
            .HasForeignKey(mf => mf.MovieId);
    }
}