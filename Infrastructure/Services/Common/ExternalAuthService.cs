using System.Security.Claims;
using custom_chat_backend.Core.Interfaces.Auth;
using Microsoft.AspNetCore.Authentication;

namespace custom_chat_backend.Infrastructure.Services.Common;

public class ExternalAuthService(IHttpContextAccessor httpContextAccessor) : IExternalAuthService
{
    private const string ExternalCookieScheme = "ExternalCookie";
    private HttpContext HttpContext => httpContextAccessor.HttpContext 
                                       ?? throw new InvalidOperationException("No se puede acceder al HttpContext fuera de una petición web.");

    public async Task<IEnumerable<Claim>> GetExternalUserClaimsAsync()
    {
        var authResult = await HttpContext.AuthenticateAsync(ExternalCookieScheme);
        
        return authResult?.Principal?.Identities.FirstOrDefault()?.Claims 
               ?? Enumerable.Empty<Claim>();
    }

    public async Task SignOutExternalAsync()
    {
        await HttpContext.SignOutAsync(ExternalCookieScheme);
    }
}