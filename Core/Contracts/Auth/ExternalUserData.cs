namespace custom_chat_backend.Core.Contracts.Auth;

public record ExternalUserData(string ChannelName,string ProviderId, string Email, string Username, string AvatarUrl);