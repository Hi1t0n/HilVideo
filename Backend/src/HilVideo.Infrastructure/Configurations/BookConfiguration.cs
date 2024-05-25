using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация таблицы Book
/// </summary>
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(b => b.BookId);
        builder.HasIndex(b => b.BookId).IsUnique();
        builder.Property(b => b.BookId).IsRequired();
        builder.Property(b => b.PosterFilePath).IsRequired();
        builder.Property(b => b.BookFilePath).IsRequired();
        builder.Property(b => b.BookName).IsRequired();
        builder.Property(b => b.BookDescription).IsRequired();
        builder.Property(b => b.ReleaseDate).IsRequired();
        
        builder.HasMany(b => b.Authors)
            .WithMany(a => a.Books)
            .UsingEntity<BookAuthor>(j => j.HasOne(ba => ba.Author)
                    .WithMany()
                    .HasForeignKey(ba => ba.AuthorId),
                j => j.HasOne(ba => ba.Book)
                    .WithMany()
                    .HasForeignKey(ba => ba.BookId),
                j => j.HasKey(ba => new { ba.BookId, ba.AuthorId  }));

        builder.HasMany(b => b.Genres)
            .WithMany(g => g.Books)
            .UsingEntity<BookGenre>(j => j.HasOne(bg => bg.Genre)
                    .WithMany()
                    .HasForeignKey(bg => bg.GenreId),
                j => j.HasOne(bg => bg.Book)
                    .WithMany()
                    .HasForeignKey(bg => bg.BookId),
                j => j.HasKey(bg => new { bg.BookId, bg.GenreId  }));
        
        builder.HasMany(u => u.Users)
            .WithMany(b => b.Books)
            .UsingEntity<FavoriteBooksUsers>(
                j => j.HasOne(fub => fub.User)
                    .WithMany()
                    .HasForeignKey(fub => fub.UserId),
                j => j.HasOne(fub => fub.Book)
                    .WithMany()
                    .HasForeignKey(fub => fub.BookId),
                j => j.HasKey(fub => new { fub.BookId, fub.UserId  }));

    }
}