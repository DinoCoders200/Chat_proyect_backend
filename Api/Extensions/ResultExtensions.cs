using Ardalis.Result;
using FastEndpoints;

namespace custom_chat_backend.Api.Extensions;

public static class ResultExtensions
{
    public static async Task SendArdalisResultAsync<TOutput, TResponse>(
        this BaseEndpoint endpoint, 
        Result<TOutput> result, 
        Func<TOutput, TResponse> mapSuccess, 
        CancellationToken ct = default) 
        where TResponse : notnull
    {
        var httpContext = endpoint.HttpContext;

        if (result.IsSuccess)
        {
            var responseBody = mapSuccess(result.Value);
            await httpContext.Response.SendAsync(responseBody, StatusCodes.Status200OK, cancellation: ct);
            return;
        }

        var statusCode = result.Status switch
        {
            ResultStatus.NotFound => StatusCodes.Status404NotFound,
            ResultStatus.Invalid => StatusCodes.Status400BadRequest,
            ResultStatus.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultStatus.Forbidden => StatusCodes.Status403Forbidden,
            ResultStatus.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        if (result.Status == ResultStatus.Invalid && result.ValidationErrors.Any())
        {
            foreach (var error in result.ValidationErrors)
            {
                endpoint.ValidationFailures.Add(new(error.Identifier, error.ErrorMessage));
            }
            
            await httpContext.Response.SendErrorsAsync(endpoint.ValidationFailures, statusCode, cancellation: ct);
            return;
        }

        var title = GetTitleForStatus(result.Status);
        var detail = result.Errors.FirstOrDefault() ?? "An error occurred while processing your request.";
        
        var problem = new ProblemDetails(
            endpoint.ValidationFailures, 
            "https://tools.ietf.org/html/rfc7231", 
            title, 
            statusCode)
        {
            Instance = httpContext.Request.Path,
            Detail = detail 
        };
        
        await httpContext.Response.SendAsync(problem, statusCode, cancellation: ct);
    }

    private static string GetTitleForStatus(ResultStatus status) => status switch
    {
        ResultStatus.NotFound => "Not Found",
        ResultStatus.Unauthorized => "Unauthorized",
        ResultStatus.Forbidden => "Forbidden",
        ResultStatus.Conflict => "Conflict Detected",
        _ => "Internal Server Error"
    };
}