using custom_chat_backend.Core.Domain.Entities.Server;
using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class ServerEntityConfiguration : IEntityTypeConfiguration<ServerEntity>
{
    public void Configure(EntityTypeBuilder<ServerEntity> builder)
    {
        builder.ToTable("Servers");
        builder.HasKey(x => x.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        builder.Property(s => s.IconUrl)
            .HasMaxLength(2048);

        // "Servers owned by this user" — the sidebar query.
        builder.HasIndex(s => s.OwnerId);

        // Restrict: deleting a user must not silently destroy servers that other
        // members belong to. Ownership is transferred instead.
        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(s => s.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
