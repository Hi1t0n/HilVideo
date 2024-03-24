using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Models;

namespace UserService.Infrastructure.Configurations;

/// <summary>
/// Конфигурация для таблицы Roles
/// </summary>
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.RoleId);
        builder.Property(x => x.RoleName)
            .IsRequired();
        builder.HasData(
            new Role { RoleId = Guid.Parse("64141029-42fc-41f7-88eb-da0801efa3f3"), RoleName = "User" },
            new Role { RoleId = Guid.Parse("b2538ada-cd21-4e9c-9309-4b4062285da3"), RoleName = "Admin" },
            new Role { RoleId = Guid.Parse("f3def9dd-1bbe-47a0-80c1-d1bc0ab5a1f4"), RoleName = "Owner" }
        );
    }
}
