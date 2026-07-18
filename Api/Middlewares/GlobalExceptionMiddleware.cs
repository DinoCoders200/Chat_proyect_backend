using System.Diagnostics;
using FastEndpoints;
using System.Text.Json;
using Serilog.Context;

namespace custom_chat_backend.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private const string CorrelationIdHeaderName = "x-correlation-id";

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            string correlationId = context.Response.Headers[CorrelationIdHeaderName].ToString();

            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                var cleanException = ex.Demystify();
        
                _logger.LogError(cleanException, "Ocurrió una excepción no controlada en el sistema.");
            }

            await HandleExceptionAsync(context, correlationId);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, string correlationId)
    {
        context.Response.ContentType = "application/json";
        int statusCode = StatusCodes.Status500InternalServerError;
        context.Response.StatusCode = statusCode;

        var problem = new ProblemDetails(
            [], 
            "https://tools.ietf.org/html/rfc7231",
            correlationId,
            statusCode: statusCode)
        {
            Instance = context.Request.Path,
            Detail = "Ha ocurrido un error inesperado en el servidor. Por favor, reporta el traceId al soporte técnico."
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        return context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
    }
}