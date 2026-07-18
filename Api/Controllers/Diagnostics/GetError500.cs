using FastEndpoints;

namespace custom_chat_backend.Api.Controllers.Diagnostics;

public class GetError500:EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/api/diagnostics/error-500");
        AllowAnonymous();
        Description(d => d
            .WithTags("Diagnostics")
            .Produces(StatusCodes.Status500InternalServerError));
    }
    
    public override Task HandleAsync(CancellationToken ct)
    {
        throw new InvalidOperationException(
            "Diagnostics error 500"
        );
    }
}