namespace custom_chat_backend.Api.Controllers.Auth.Login;

public sealed record LoginRequest(
    string User,
    string Password)
{
    public const string Route = "/Auth/Login";
}