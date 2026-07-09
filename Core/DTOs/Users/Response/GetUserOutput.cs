namespace custom_chat_backend.Core.DTOs.Users.Response;

public sealed record GetUserOutput(
    string Username,
    string Password);