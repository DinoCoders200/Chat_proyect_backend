using System.Security.Claims;

namespace custom_chat_backend.Api.Controllers.Auth.CallbackProvider;

public record CallbackProviderRequest(string ProviderName)
{ 
    public const string Route = "/Auth/Callback/{ProviderName}";
}