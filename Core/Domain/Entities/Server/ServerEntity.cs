using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.Server;

public class ServerEntity : AuditableEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? IconUrl { get; private set; }

    /// <summary>
    /// The user that owns the server. Distinct from <see cref="AuditableEntity.CreatedBy"/>,
    /// since ownership can be transferred but the creator never changes.
    /// </summary>
    public Guid OwnerId { get; private set; }

    protected ServerEntity() { }

    public ServerEntity(string name, Guid ownerId)
    {
        Id = Guid.NewGuid();
        Name = name;
        OwnerId = ownerId;
    }
}
