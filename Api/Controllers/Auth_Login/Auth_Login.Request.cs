namespace custom_chat_backend.Api.Controllers.Auth_Login;

public sealed record Auth_LoginRequest(
    string User,
    string Password)
{
    public const string Route = "/Auth_Login";
}