using Ardalis.Result;
using FastEndpoints;
using custom_chat_backend.Api.Extensions;

namespace custom_chat_backend.Api.Controllers.Auth.Login;

public class Login
    : Endpoint<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Version(1);

        // Configura el endpoint como POST /Login
        Post(LoginRequest.Route);

        // Permite acceder al endpoint sin autenticación
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {

        // Simula la autenticación del usuario sin consultar una base de datos.
        Result<LoginResponse> result = await FakeLogin(req);

        await this.SendArdalisResultAsync(result, ct);
    }

    // Simulación de autenticación (Mockeado)
    private async Task<Result<LoginResponse>> FakeLogin(LoginRequest req)
    {
        if (req.User == "admin" &&
            req.Password == "123456")
        {
            return Result.Success(
                new LoginResponse(true));
        }

        return Result.Unauthorized();
    }
}