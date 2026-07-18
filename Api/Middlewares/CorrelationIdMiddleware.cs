using System.Runtime.CompilerServices;
using Serilog.Context;

namespace custom_chat_backend.Api.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string CorrelationIdHeaderName = "x-correlation-id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = string.IsNullOrEmpty(context.Request.Headers[CorrelationIdHeaderName])
            ? Guid.NewGuid().ToString()
            : context.Request.Headers[CorrelationIdHeaderName].ToString();

        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
    
}