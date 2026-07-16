using custom_chat_backend.Core.Domain.Entities.Common;
using custom_chat_backend.Core.Domain.Enums;

namespace custom_chat_backend.Core.Domain.Entities.OAuthAccount;

public class OAuthAccountEntity : AuditableEntity, ISoftDeletable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public OAuthProvider Provider { get; private set; }

    /// <summary>
    /// The user's id at the provider. Unique per provider.
    /// </summary>
    public string ProviderUserId { get; private set; } = string.Empty;

    public string? ProviderEmail { get; private set; }
    public string? AccessToken { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? TokenExpiration { get; private set; }
    public DateTime? DeletedAt { get; set; }

    protected OAuthAccountEntity() { }

    public OAuthAccountEntity(Guid userId, OAuthProvider provider, string providerUserId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Provider = provider;
        ProviderUserId = providerUserId;
    }
}
