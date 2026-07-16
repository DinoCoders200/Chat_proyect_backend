using custom_chat_backend.Core.Domain.Entities.OAuthAccount;
using custom_chat_backend.Core.Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class OAuthAccountEntityConfiguration : IEntityTypeConfiguration<OAuthAccountEntity>
{
    public void Configure(EntityTypeBuilder<OAuthAccountEntity> builder)
    {
        builder.ToTable("OAuthAccounts");
        builder.HasKey(x => x.Id);

        builder.Property(o => o.Provider)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.ProviderUserId)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(o => o.ProviderEmail)
            .HasMaxLength(255);

        builder.Property(o => o.AccessToken)
            .HasColumnType("text");

        builder.Property(o => o.RefreshToken)
            .HasColumnType("text");

        builder.Property(o => o.TokenExpiration)
            .HasColumnType("timestamp with time zone");

        // The OAuth callback looks up exactly this pair; it must also be unique so the
        // same provider account cannot be linked twice.
        builder.HasIndex(o => new { o.Provider, o.ProviderUserId })
            .IsUnique();

        builder.HasOne<UserEntity>()
            .WithMany()
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
