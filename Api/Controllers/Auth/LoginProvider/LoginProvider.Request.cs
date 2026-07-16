using System.Security.Claims;

namespace custom_chat_backend.Api.Controllers.Auth.LoginProvider;

public record LoginProviderRequest(string Provider, IEnumerable<Claim> Claims)
{
    public const string Route = "/Auth/Login/{Provider}";
}