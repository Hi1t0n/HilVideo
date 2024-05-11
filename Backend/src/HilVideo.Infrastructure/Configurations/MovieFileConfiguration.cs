using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация для таблицы MovieFiles
/// </summary>
public class MovieFileConfiguration : IEntityTypeConfiguration<MovieFile>
{
    public void Configure(EntityTypeBuilder<MovieFile> builder)
    {
        builder.HasKey(mf => mf.MovieFileId);
        builder.HasIndex(mf => mf.MovieFileId).IsUnique();
        builder.Property(mf => mf.MovieFileId).IsRequired();
        builder.Property(mf => mf.MovieId).IsRequired();
        builder.Property(mf => mf.FilePath).IsRequired();
        builder.HasIndex(mf => mf.FilePath).IsUnique();
        builder.Property(mf => mf.EpisodNumber).HasDefaultValue(1);
    }
}