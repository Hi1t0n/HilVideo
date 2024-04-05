using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы MovieDirector
/// </summary>
public class MovieDirectorConfiguration : IEntityTypeConfiguration<MovieDirector>
{
    public void Configure(EntityTypeBuilder<MovieDirector> builder)
    {
        builder.HasKey(md => new { md.MovieId, md.DirectorId });

        builder.HasOne(md => md.Movie)
            .WithMany(m=> m.MovieDirectors)
            .HasForeignKey(md => md.MovieId);

        builder.HasOne(md => md.Director)
            .WithMany(d=>d.MovieDirectors)
            .HasForeignKey(md => md.DirectorId);
    }
}