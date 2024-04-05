using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация для таблицы MovieType
/// </summary>
public class MovieTypeConfiguration : IEntityTypeConfiguration<MovieType>
{
    public void Configure(EntityTypeBuilder<MovieType> builder)
    {
        builder.HasKey(mt => mt.MovieTypeId);
        builder.Property(mt => mt.MovieTypeId).IsRequired();
        builder.HasIndex(mt => mt.MovieTypeId).IsUnique();
        builder.Property(mt => mt.MovieTypeName).IsRequired();
        builder.HasIndex(mt => mt.MovieTypeName).IsUnique();
    }
}