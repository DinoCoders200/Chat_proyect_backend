using custom_chat_backend.Core.Domain.Entities.Common;
using custom_chat_backend.Core.Domain.Entities.Permission.Enums;

namespace custom_chat_backend.Core.Domain.Entities.Permission;

/// <summary>
/// A permission granted to a single server role. Roles own their permission rows
/// (server_roles 1:N Permissions); there is no shared permission catalogue.
/// </summary>
public class PermissionEntity : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid ServerRoleId { get; private set; }
    public PermissionType Permission { get; private set; }
    public string? Description { get; private set; }

    protected PermissionEntity() { }

    public PermissionEntity(Guid serverRoleId, PermissionType permission)
    {
        Id = Guid.NewGuid();
        ServerRoleId = serverRoleId;
        Permission = permission;
    }
}
