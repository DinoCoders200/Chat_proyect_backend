using FastEndpoints; 
using System.Text.Json;
using Serilog.Context;

namespace custom_chat_backend.Api.Middlewares
{
    public class DatabaseExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DatabaseExceptionMiddleware> _logger;
        private const string CorrelationIdHeaderName = "x-correlation-id";
        

        public DatabaseExceptionMiddleware(RequestDelegate next, ILogger<DatabaseExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex) when (IsDatabaseError(ex))
            {
                string correlationId = httpContext.Response.Headers[CorrelationIdHeaderName].ToString();

                using (LogContext.PushProperty("CorrelationId", correlationId))
                {
                    _logger.LogCritical(ex, "Error crítico de conexión con la base de datos (PostgreSQL).");
                }
    
                await HandleDatabaseExceptionAsync(httpContext, correlationId);
            }
        }

        private static bool IsDatabaseError(Exception ex)
        {
            string fullExceptionText = ex.ToString();
            return fullExceptionText.Contains("NpgsqlException") || 
                   fullExceptionText.Contains("SocketException") ||
                   fullExceptionText.Contains("RelationalConnection");
        }

        private static Task HandleDatabaseExceptionAsync(HttpContext context, string correlationId)
        {
            context.Response.ContentType = "application/json";
            
            int statusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails(
                new List<FluentValidation.Results.ValidationFailure>(),
                "https://tools.ietf.org/html/rfc7231",
                correlationId,
                 statusCode)
            {
                Instance = context.Request.Path,
                Detail = "ERROR_PERSISTENCE_CONNECTION",
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
        }
    }
}