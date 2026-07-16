using custom_chat_backend.Api.Extensions;
using custom_chat_backend.Core.Application.UseCases.Auth.Queries;
using custom_chat_backend.Core.Interfaces.Auth;
using FastEndpoints;
using MediatR;

namespace custom_chat_backend.Api.Controllers.Auth.CallbackProvider;

public class CallbackProvider(IMediator mediator, IExternalAuthService externalAuthService):Endpoint<CallbackProviderRequest,CallbackProviderResponse>
{
    public override void Configure()
    {
        Version(1);
        Get(CallbackProviderRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CallbackProviderRequest req, CancellationToken ct)
    {
        var claims = await externalAuthService.GetExternalUserClaimsAsync();
        var input = req.ToInput(claims);
        var res= await mediator.Send(new CallbackQuery(input), ct);

        await this.SendArdalisResultAsync(res, x => x.ToResponse(), ct);
    }
}