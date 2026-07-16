using custom_chat_backend.Core.Domain.Entities.LoginLog;
using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class LoginLogEntityConfiguration : IEntityTypeConfiguration<LoginLogEntity>
{
    public void Configure(EntityTypeBuilder<LoginLogEntity> builder)
    {
        builder.ToTable("LoginLogs");
        builder.HasKey(x => x.Id);

        builder.Property(l => l.Provider)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.EventType)
            .IsRequired()
            .HasMaxLength(50);

        // Wide enough for IPv6.
        builder.Property(l => l.IpAddress)
            .HasMaxLength(45);

        builder.Property(l => l.UserAgent)
            .HasMaxLength(512);

        builder.Property(l => l.DeviceName)
            .HasMaxLength(100);

        builder.Property(l => l.Country)
            .HasMaxLength(100);

        builder.Property(l => l.City)
            .HasMaxLength(100);

        builder.Property(l => l.Success)
            .IsRequired();

        builder.Property(l => l.FailureReason)
            .HasMaxLength(255);

        // "Recent login attempts for this user, newest first" is the only read pattern.
        builder.HasIndex(l => new { l.UserId, l.CreatedAt })
            .IsDescending(false, true);

        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
