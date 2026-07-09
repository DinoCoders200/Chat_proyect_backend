using custom_chat_backend.Api.Extensions;
using custom_chat_backend.Core.Application.UseCases.User.Queries;
using FastEndpoints;
using MediatR;

namespace custom_chat_backend.Api.Controllers.User.GetUser;

public class GetUser(IMediator mediator)
    : Endpoint<GetUserRequest, GetUserResponse>
{
    public override void Configure()
    {
        Version(1);
        Get(GetUserRequest.Route);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
    {
        var input = req.ToInput();
        var res = await mediator.Send(new GetUserQuery(input), ct);
        await this.SendArdalisResultAsync(res, x => x.ToOutput(), ct);
    }
}