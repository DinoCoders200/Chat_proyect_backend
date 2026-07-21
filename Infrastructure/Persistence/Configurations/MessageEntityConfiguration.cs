using custom_chat_backend.Core.Domain.Entities.Channel;
using custom_chat_backend.Core.Domain.Entities.Message;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace custom_chat_backend.Infrastructure.Persistence.Configurations;

public class MessageEntityConfiguration : IEntityTypeConfiguration<MessageEntity>
{
    public void Configure(EntityTypeBuilder<MessageEntity> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(x => x.Id);

        builder.Property(m => m.Content)
            .IsRequired()
            .HasColumnType("text");

        // The hot path of the whole application: "last N messages of this channel,
        // newest first". A single-column index on ChannelId cannot serve the ordering,
        // so the sort has to be part of the index.
        builder.HasIndex(m => new { m.ChannelId, m.CreatedAt })
            .IsDescending(false, true);

        // Deleting a channel deletes its messages.
        builder.HasOne<ChannelEntity>()
            .WithMany()
            .HasForeignKey(m => m.ChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
