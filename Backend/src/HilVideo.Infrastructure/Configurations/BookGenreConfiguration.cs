using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
{
    public void Configure(EntityTypeBuilder<BookGenre> builder)
    {
        builder.HasKey(bg => new { bg.BookId, bg.GenreId });
        
        /* Связь один ко многим между таблицами Genre и BookGenre */
        builder.HasOne(bg => bg.Book)
            .WithMany(bg => bg.BookGenres)
            .HasForeignKey(bg => bg.BookId);
        
        /* Связь один ко многим между таблицами Genre и BookGenres */
        builder.HasOne(bg => bg.Genre)
            .WithMany(bg => bg.BookGenres)
            .HasForeignKey(bg => bg.GenreId);
    }
}