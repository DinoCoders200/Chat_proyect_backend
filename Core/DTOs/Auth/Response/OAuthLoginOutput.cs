namespace custom_chat_backend.Core.DTOs.Auth.Response;

public record OAuthLoginOutput(
    string AccessToken,
    string RefreshToken);