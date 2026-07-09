using Ardalis.Specification;
using custom_chat_backend.Api.Middlewares;
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
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(EfRepository<>));
var app = builder.Build();

app.MapHealthChecks("/healthz");
app.UseMiddleware<DatabaseExceptionMiddleware>();
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