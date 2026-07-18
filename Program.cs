using Ardalis.Specification;
using custom_chat_backend.Api.Middlewares;
using custom_chat_backend.Core.Application.Strategies.Auth;
using custom_chat_backend.Core.Interfaces.Auth;
using custom_chat_backend.Infrastructure.Persistence.Context;
using custom_chat_backend.Infrastructure.Services.Common;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpContextAccessor();
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.Configuration["ASPNETCORE_HTTP_PORTS"] = port;

builder.Services.AddHealthChecks(); 
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();
builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie("ExternalCookie") 
.AddDiscord("discord", options => 
{
    options.ClientId = builder.Configuration["Authentication:Discord:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Discord:ClientSecret"]!;
    options.SignInScheme = "ExternalCookie";
    options.CallbackPath = new PathString("/auth/login/signin-discord");
    options.Scope.Add("identify");
    options.Scope.Add("email");
});
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(EfRepository<>));
builder.Services.AddScoped<IExternalAuthProviderStrategy, DiscordAuthStrategy>();
builder.Services.AddScoped<IExternalAuthService, ExternalAuthService>();
builder.Services.AddScoped<AuthProviderFactory>();
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console(new Serilog.Templates.ExpressionTemplate(
        "[{@t:yyyy-MM-dd HH:mm:ss.fff} {@l:u3}] {@m} [ID: {CorrelationId}]\n{@x}"
    ))
);
var app = builder.Build();

app.MapHealthChecks("/healthz");
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<DatabaseExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseSerilogRequestLogging();

app.MapOpenApi();            
app.MapScalarApiReference();
app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";         
    c.Versioning.PrependToRoute = true; 
    c.Errors.UseProblemDetails(); 

});
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); 
}
app.UseAuthorization(); 

app.MapGet("/", () => Results.Redirect("/scalar", permanent: true));

app.Run();