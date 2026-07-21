using custom_chat_backend.Core.Domain.Entities.Server;
using custom_chat_backend.Core.Domain.Entities.ServerMember;
using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class ServerMemberEntityConfiguration : IEntityTypeConfiguration<ServerMemberEntity>
{
    public void Configure(EntityTypeBuilder<ServerMemberEntity> builder)
    {
        builder.ToTable("ServerMembers");
        builder.HasKey(x => x.Id);

        builder.Property(m => m.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(m => m.JoinedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        // Membership checks run on every request against a server, and a user must not be
        // able to join the same server twice.
        builder.HasIndex(m => new { m.ServerId, m.UserId })
            .IsUnique();

        // "Servers this user belongs to" — the sidebar query.
        builder.HasIndex(m => m.UserId);

        builder.HasOne<ServerEntity>()
            .WithMany()
            .HasForeignKey(m => m.ServerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: Servers.OwnerId already restricts, and cascading here would give
        // Postgres two delete paths from Users into this table.
        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
