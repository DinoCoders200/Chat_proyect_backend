using custom_chat_backend.Core.Domain.Entities.Permission;
using custom_chat_backend.Core.Domain.Entities.ServerRole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class PermissionEntityConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Permission)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        // A role must not hold the same permission twice.
        builder.HasIndex(p => new { p.ServerRoleId, p.Permission })
            .IsUnique();

        builder.HasOne<ServerRoleEntity>()
            .WithMany()
            .HasForeignKey(p => p.ServerRoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
