using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Authentication;

namespace custom_chat_backend.Api.Controllers.Auth.LoginProvider;

public class LoginProvider():Endpoint<LoginProviderRequest,GetLoginCallbackResponse>
{
    public override void Configure()
    {
        Version(1);
        Get(LoginProviderRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginProviderRequest callbackRequest, CancellationToken cancellationToken)
    {
        var input = callbackRequest.ToInput();
        
        var properties = new AuthenticationProperties
        {
            RedirectUri = $"/v1/auth/callback/{input.ProviderName}"
        };

        await HttpContext.ChallengeAsync(input.ProviderName, properties);
        
    }
}