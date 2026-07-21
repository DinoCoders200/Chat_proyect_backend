using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.Message;

/// <summary>
/// A message in a channel. The author is <see cref="AuditableEntity.CreatedBy"/>; the
/// relational model has no separate author column.
/// </summary>
public class MessageEntity : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public Guid ChannelId { get; private set; }
    public string Content { get; private set; } = string.Empty;
    public DateTime? DeletedAt { get; set; }

    protected MessageEntity() { }

    public MessageEntity(Guid channelId, string content)
    {
        Id = Guid.NewGuid();
        ChannelId = channelId;
        Content = content;
    }
}
