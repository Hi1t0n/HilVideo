using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы Author
/// </summary>
public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(a => a.AuthorId);
        builder.HasIndex(a => a.AuthorId).IsUnique();
        builder.Property(a => a.AuthorId).IsRequired();
        builder.Property(a => a.FirstName).IsRequired();
        builder.Property(a => a.SecondName).IsRequired();
        builder.HasMany(a => a.Books)
            .WithMany(b => b.Authors)
            .UsingEntity<BookAuthor>(
                j => j.HasOne(ba => ba.Book)
                    .WithMany()
                    .HasForeignKey(ba => ba.BookId),
                j => j.HasOne(ba => ba.Author)
                    .WithMany()
                    .HasForeignKey(ba => ba.AuthorId),
                j => j.HasKey(ba => new{ba.BookId, ba.AuthorId}));
    }
}