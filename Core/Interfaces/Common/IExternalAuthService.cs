using System.Security.Claims;

namespace custom_chat_backend.Core.Interfaces.Auth;

public interface IExternalAuthService
{
    // <summary>
    /// Recupera los claims del usuario desde la cookie temporal del proveedor externo.
    /// </summary>
    Task<IEnumerable<Claim>> GetExternalUserClaimsAsync();
    
    /// <summary>
    /// Limpia la sesión y cookies temporales utilizadas durante el flujo de OAuth.
    /// </summary>
    Task SignOutExternalAsync();
}