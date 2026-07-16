using custom_chat_backend.Core.DTOs.Auth.Request;
using custom_chat_backend.Core.DTOs.Auth.Response;

namespace custom_chat_backend.Api.Controllers.Auth.LoginProvider;

public static class LoginProviderMapper
{
    public static GetLoginCallbackInput ToInput(this LoginProviderRequest req)
    {
        return new GetLoginCallbackInput(req.Provider, req.Claims);
    }

    public static GetLoginCallbackResponse ToResponse(this GetLoginCallbackOutput output)
    {
        return new GetLoginCallbackResponse(output.ProviderName,output.Email);
    }
}