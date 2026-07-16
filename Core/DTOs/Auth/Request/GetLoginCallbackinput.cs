
using System.Security.Claims;

namespace custom_chat_backend.Core.DTOs.Auth.Request;

public record GetLoginCallbackInput(string ProviderName, IEnumerable<Claim> Claims);