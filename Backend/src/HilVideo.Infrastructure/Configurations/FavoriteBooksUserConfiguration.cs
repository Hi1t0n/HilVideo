using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

public class FavoriteBooksUserConfiguration : IEntityTypeConfiguration<FavoriteBooksUsers>
{
    public void Configure(EntityTypeBuilder<FavoriteBooksUsers> builder)
    {
        builder.HasKey(fub => new { fub.BookId, fub.UserId });
        
        builder.HasOne(fub => fub.Book)
            .WithMany(b => b.FavoriteBooksUsers)
            .HasForeignKey(fub => fub.BookId);

        builder.HasOne(fub => fub.User)
            .WithMany(u => u.FavoriteBooksUsers)
            .HasForeignKey(fub => fub.UserId);
    }
}