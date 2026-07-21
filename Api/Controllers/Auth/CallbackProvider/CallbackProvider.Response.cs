namespace custom_chat_backend.Api.Controllers.Auth.CallbackProvider;

public record CallbackProviderResponse(string AccessToken, string RefreshToken);