using Ardalis.Specification;
using custom_chat_backend.Core.Domain.Entities.OAuthAccount.Enums;

namespace custom_chat_backend.Core.Domain.Entities.OAuthAccount.Specifications;

public class OAuthAccountByProviderSpec : Specification<OAuthAccountEntity>
{
    public OAuthAccountByProviderSpec(
        OAuthProvider provider,
        string providerUserId,
        bool asNoTracking = true)
    {
        var query = Query.Where(x =>
            x.Provider == provider &&
            x.ProviderUserId == providerUserId);

        if (asNoTracking)
        {
            query.AsNoTracking();
        }
    }
}