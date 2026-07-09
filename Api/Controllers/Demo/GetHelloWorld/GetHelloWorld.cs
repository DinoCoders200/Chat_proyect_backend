using FastEndpoints;
using Ardalis.Result;
using custom_chat_backend.Api.Extensions;

namespace custom_chat_backend.Api.Controllers.Demo.GetHelloWorld;

public class GetHelloWorld
    :Endpoint<GetHelloWorldRequest,GetHelloWorldResponse>
{
    public override void Configure()
    {
        Version(1);
        Get(GetHelloWorldRequest.Route);
        AllowAnonymous();
    }
    
    public override async Task HandleAsync(GetHelloWorldRequest req, CancellationToken ct)
    {
        Result<GetHelloWorldResponse> result = await FakeServiceGetHello(req);

        await this.SendArdalisResultAsync(result,x=>x.Response, ct);
    }

    private async Task<Result<GetHelloWorldResponse>> FakeServiceGetHello(GetHelloWorldRequest req)
    {
        var response = new GetHelloWorldResponse("Hello World");
        return Result.Success(response);
    }
    
}