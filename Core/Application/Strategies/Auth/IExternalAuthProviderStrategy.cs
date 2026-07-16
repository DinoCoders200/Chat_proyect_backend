using System.Security.Claims;
using custom_chat_backend.Core.Contracts.Auth;

namespace custom_chat_backend.Core.Application.Strategies.Auth;

public interface IExternalAuthProviderStrategy
{
    string ProviderName { get; }
    ExternalUserData ExtractUserData(IEnumerable<Claim> claims);
}