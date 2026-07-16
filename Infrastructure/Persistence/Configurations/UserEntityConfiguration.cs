using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class UserEntityConfiguration:IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.EmailVerified)
            .IsRequired()
            .HasDefaultValue(false);

        // Null for accounts that only authenticate through an external provider.
        builder.Property(u => u.Password)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(u => u.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.AccountRole)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(u => u.LastLoginAt)
            .HasColumnType("timestamp with time zone");
    }
}
