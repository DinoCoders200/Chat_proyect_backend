using System.Security.Claims;
using custom_chat_backend.Core.DTOs.Auth.Request;
using custom_chat_backend.Core.DTOs.Auth.Response;

namespace custom_chat_backend.Api.Controllers.Auth.CallbackProvider;

public static class CallbackProviderMapper
{
    public static GetLoginCallbackInput ToInput(this CallbackProviderRequest req, IEnumerable<Claim>  claims)
    {
        return new GetLoginCallbackInput(req.ProviderName,claims);
    }

    public static CallbackProviderResponse ToResponse(this GetLoginCallbackOutput output)
    {
        return new CallbackProviderResponse(output.ProviderName,output.Email);
    }
}