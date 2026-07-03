using custom_chat_backend.Infrastructure.Persistence.Context;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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

var app = builder.Build();

app.MapHealthChecks("/healthz"); 

app.MapOpenApi();            
app.MapScalarApiReference();

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); 
}

app.UseAuthorization(); 
app.UseFastEndpoints(config =>
{
    config.Errors.UseProblemDetails(); 
});

app.MapGet("/", () => Results.Redirect("/scalar", permanent: true));

app.Run();