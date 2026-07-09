using FastEndpoints; 
using System.Text.Json;

namespace custom_chat_backend.Api.Middlewares
{
    public class DatabaseExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DatabaseExceptionMiddleware> _logger;

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
                _logger.LogCritical(ex, "Error crítico de conexión con la base de datos (PostgreSQL).");
                await HandleDatabaseExceptionAsync(httpContext);
            }
        }

        private static bool IsDatabaseError(Exception ex)
        {
            string fullExceptionText = ex.ToString();
            return fullExceptionText.Contains("NpgsqlException") || 
                   fullExceptionText.Contains("SocketException") ||
                   fullExceptionText.Contains("RelationalConnection");
        }

        private static Task HandleDatabaseExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            
            int statusCode = StatusCodes.Status503ServiceUnavailable;
            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails(
                failures: new List<FluentValidation.Results.ValidationFailure>(), // Lista vacía (no es error de validación)
                statusCode: statusCode)
            {
                Instance = context.Request.Path,
                Detail = "ERROR_PERSISTENCE_CONNECTION",
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
        }
    }
}