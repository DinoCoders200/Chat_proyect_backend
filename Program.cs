using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurar el puerto de forma nativa para el contenedor
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// 2. Agregar los servicios de Health Checks nativos
builder.Services.AddHealthChecks(); 

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();

// 3. Registrar el endpoint de salud en la raíz o en /health
// Esto responderá un HTTP 200 (OK) instantáneo que la nube podrá leer
app.MapHealthChecks("/health"); 

app.MapOpenApi();            
app.MapScalarApiReference();

// 4. Evitar bucles de redirección HTTPS en la nube
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); 
}

app.UseAuthorization(); 
app.UseFastEndpoints(config =>
{
    config.Errors.UseProblemDetails(); 
});

app.Run(); // Arranca limpio leyendo la configuración de Kestrel