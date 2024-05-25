using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация для таблицы Users
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.Login)
            .HasMaxLength(30)
            .IsRequired();
        builder.HasIndex(x => x.Login).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
        builder.Property(x => x.RoleId)
            .HasDefaultValue(new Guid("64141029-42fc-41f7-88eb-da0801efa3f3"));
        builder.Property(x => x.Password)
            .IsRequired();
        
        /* Связь многие ко многим Movie и User (FavoriteMoviesUsers) */
        builder.HasMany(u => u.Movies)
            .WithMany(m => m.Users)
            .UsingEntity<FavoriteMoviesUsers>(
                j => j.HasOne(fum => fum.Movie)
                    .WithMany()
                    .HasForeignKey(fum => fum.MovieId),
                j => j.HasOne(fum => fum.User)
                    .WithMany()
                    .HasForeignKey(fum => fum.UserId),
                j => j.HasKey(fum => new { fum.UserId, fum.MovieId })
            );

        builder.HasMany(u => u.Books)
            .WithMany(b => b.Users)
            .UsingEntity<FavoriteBooksUsers>(
                j => j.HasOne(fub => fub.Book)
                    .WithMany()
                    .HasForeignKey(fub => fub.BookId),
                j => j.HasOne(fub => fub.User)
                    .WithMany()
                    .HasForeignKey(fub => fub.UserId),
                j => j.HasKey(fub => new { fub.UserId, fub.BookId }));
        
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
    }
}
