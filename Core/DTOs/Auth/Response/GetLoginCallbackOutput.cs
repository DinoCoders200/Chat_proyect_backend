namespace custom_chat_backend.Core.DTOs.Auth.Response;

public record GetLoginCallbackOutput(string ProviderName, string Email);