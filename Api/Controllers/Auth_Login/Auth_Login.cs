using Ardalis.Result;
using FastEndpoints;
using custom_chat_backend.Api.Extensions;

namespace custom_chat_backend.Api.Controllers.Auth_Login;

public class Login
    : Endpoint<Auth_LoginRequest, Auth_LoginResponse>
{
    public override void Configure()
    {
        Version(1);

        // Configura el endpoint como POST /Login
        Post(Auth_LoginRequest.Route);

        // Permite acceder al endpoint sin autenticación
        AllowAnonymous();
    }

    public override async Task HandleAsync(Auth_LoginRequest req, CancellationToken ct)
    {

        // Simula la autenticación del usuario sin consultar una base de datos.
        Result<Auth_LoginResponse> result = await FakeLogin(req);

        await this.SendArdalisResultAsync(result, ct);
    }

    // Simulación de autenticación (Mockeado)
    private async Task<Result<Auth_LoginResponse>> FakeLogin(Auth_LoginRequest req)
    {
        if (req.User == "admin" &&
            req.Password == "123456")
        {
            return Result.Success(
                new Auth_LoginResponse(true));
        }

        return Result.Unauthorized();
    }
}