using custom_chat_backend.Core.Domain.Entities.Common;
using custom_chat_backend.Core.Domain.Enums;

namespace custom_chat_backend.Core.Domain.Entities.User;

public class UserEntity : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool EmailVerified { get; private set; }

    /// <summary>
    /// Null for accounts that authenticate only through an external provider.
    /// </summary>
    public string? Password { get; private set; }

    public UserStatus Status { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public AccountRole AccountRole { get; private set; }
    public DateTime? DeletedAt { get; set; }

    protected UserEntity() { }

    public UserEntity(string username, string email, string? password = null)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        Password = password;
        Status = UserStatus.Active;
        AccountRole = AccountRole.User;
    }
}
