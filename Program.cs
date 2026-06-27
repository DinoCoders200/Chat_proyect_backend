using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

builder.Services.AddHealthChecks(); 

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.MapHealthChecks("/health"); 

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

app.Run();