using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы Director
/// </summary>
public class DirectorConfiguration :  IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.HasKey(d => d.DirectorId);
        builder.Property(d => d.DirectorId).IsRequired();
        builder.HasIndex(d => d.DirectorId).IsUnique();
        builder.Property(d => d.FirstName).IsRequired();
        builder.Property(d => d.SecondName).IsRequired();
        builder.HasMany(d => d.Movies)
            .WithMany(m => m.Directors)
            .UsingEntity<MovieDirector>(
                j => j.HasOne(md => md.Movie)
                    .WithMany()
                    .HasForeignKey(md => md.MovieId),
                j => j.HasOne(md => md.Director)
                    .WithMany()
                    .HasForeignKey(md => md.DirectorId),
                j => j.HasKey(md => new {md.MovieId, md.DirectorId})
            );
    }
}