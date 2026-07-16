using System.Security.Claims;
using custom_chat_backend.Core.Contracts.Auth;

namespace custom_chat_backend.Core.Application.Strategies.Auth;

public class DiscordAuthStrategy:IExternalAuthProviderStrategy
{
    public string ProviderName => "discord";

    public ExternalUserData ExtractUserData(IEnumerable<Claim> claims)
    {
        var list = claims.ToList();
        var email = list.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var discordId = list.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var username = list.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        
        var avatarHash = list.FirstOrDefault(c => c.Type == "urn:discord:avatar:hash")?.Value;
        string avatarUrl = string.IsNullOrEmpty(avatarHash) 
            ? "https://cdn.discordapp.com/embed/avatars/0.png" 
            : $"https://cdn.discordapp.com/avatars/{discordId}/{avatarHash}.png";
        
        return new ExternalUserData("discord", discordId!, email!, username!, avatarUrl);
    }
}