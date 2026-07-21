using custom_chat_backend.Core.Domain.Entities.Common;

namespace custom_chat_backend.Core.Domain.Entities.UserRole;

/// <summary>
/// Assignment of a server role to a user. Carries its own key and audit columns, so it is
/// an explicit entity rather than an implicit many-to-many join.
/// </summary>
public class UserRoleEntity : AuditableEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid ServerRolesId { get; private set; }

    protected UserRoleEntity() { }

    public UserRoleEntity(Guid userId, Guid serverRolesId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ServerRolesId = serverRolesId;
    }
}
