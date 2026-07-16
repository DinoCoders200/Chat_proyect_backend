using custom_chat_backend.Core.Domain.Entities.Server;
using custom_chat_backend.Core.Domain.Entities.ServerRole;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class ServerRoleEntityConfiguration : IEntityTypeConfiguration<ServerRoleEntity>
{
    public void Configure(EntityTypeBuilder<ServerRoleEntity> builder)
    {
        builder.ToTable("ServerRoles");
        builder.HasKey(x => x.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.IsDefault)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        // "#RRGGBB"
        builder.Property(r => r.Color)
            .HasMaxLength(7);

        // Role names are shown per server and must not collide within one.
        builder.HasIndex(r => new { r.ServerId, r.Name })
            .IsUnique();

        builder.HasOne<ServerEntity>()
            .WithMany()
            .HasForeignKey(r => r.ServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
