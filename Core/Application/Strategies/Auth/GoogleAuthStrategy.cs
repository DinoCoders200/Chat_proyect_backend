using System.Security.Claims;
using custom_chat_backend.Core.Contracts.Auth;

namespace custom_chat_backend.Core.Application.Strategies.Auth;

public class GoogleAuthStrategy : IExternalAuthProviderStrategy
{
    public string ProviderName => "google";

    public ExternalUserData ExtractUserData(IEnumerable<Claim> claims)
    {
        var list = claims.ToList();

        var email = list.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var googleId = list.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var username = list.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        var avatarUrl = list.FirstOrDefault(c => c.Type == "picture")?.Value ?? string.Empty;

        if (string.IsNullOrWhiteSpace(googleId) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException(
                "Google claims are missing required information.");
        }

        return new ExternalUserData(
            ProviderName,
            googleId,
            email,
            username,
            avatarUrl);
    }
}