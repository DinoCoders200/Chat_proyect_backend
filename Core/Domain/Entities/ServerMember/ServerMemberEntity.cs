using custom_chat_backend.Core.Domain.Entities.Common;
using custom_chat_backend.Core.Domain.Entities.ServerMember.Enums;

namespace custom_chat_backend.Core.Domain.Entities.ServerMember;

/// <summary>
/// Membership of a user in a server. Carries its own key and payload, so it is an
/// explicit entity rather than an implicit many-to-many join.
/// </summary>
public class ServerMemberEntity : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid ServerId { get; private set; }
    public Guid UserId { get; private set; }
    public ServerMemberRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }

    protected ServerMemberEntity() { }

    public ServerMemberEntity(Guid serverId, Guid userId, ServerMemberRole role)
    {
        Id = Guid.NewGuid();
        ServerId = serverId;
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
    }
}
