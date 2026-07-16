using custom_chat_backend.Core.Domain.Entities.Channel;
using custom_chat_backend.Core.Domain.Entities.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class ChannelEntityConfiguration : IEntityTypeConfiguration<ChannelEntity>
{
    public void Configure(EntityTypeBuilder<ChannelEntity> builder)
    {
        builder.ToTable("Channels");
        builder.HasKey(x => x.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.Topic)
            .HasMaxLength(1024);

        builder.Property(c => c.Position)
            .IsRequired();

        // "Channels of this server, in display order" — runs on every server open.
        builder.HasIndex(c => new { c.ServerId, c.Position });

        // Deleting a server deletes its channels.
        builder.HasOne<ServerEntity>()
            .WithMany()
            .HasForeignKey(c => c.ServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
