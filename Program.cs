using FastEndpoints;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();


app.MapOpenApi();            
app.MapScalarApiReference();  


app.UseHttpsRedirection();
app.UseAuthorization(); 
app.UseFastEndpoints(config =>
{
    config.Errors.UseProblemDetails(); 
});
app.Run();