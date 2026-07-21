using custom_chat_backend.Core.Domain.Entities.Common;
using custom_chat_backend.Core.Domain.Entities.Channel.Enums;

namespace custom_chat_backend.Core.Domain.Entities.Channel;

public class ChannelEntity : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid ServerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public ChannelType Type { get; private set; }
    public string? Topic { get; private set; }

    /// <summary>
    /// Ordering of the channel inside its server.
    /// </summary>
    public int Position { get; private set; }

    protected ChannelEntity() { }

    public ChannelEntity(Guid serverId, string name, ChannelType type, int position = 0)
    {
        Id = Guid.NewGuid();
        ServerId = serverId;
        Name = name;
        Type = type;
        Position = position;
    }
}
