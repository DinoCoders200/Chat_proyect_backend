using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.ServerRole;

public class ServerRoleEntity : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid ServerId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// The role automatically granted to new members of the server.
    /// </summary>
    public bool IsDefault { get; private set; }

    public string? Description { get; private set; }

    /// <summary>
    /// Hex colour used to render the role, e.g. "#5865F2".
    /// </summary>
    public string? Color { get; private set; }

    protected ServerRoleEntity() { }

    public ServerRoleEntity(Guid serverId, string name, bool isDefault = false)
    {
        Id = Guid.NewGuid();
        ServerId = serverId;
        Name = name;
        IsDefault = isDefault;
    }
}
