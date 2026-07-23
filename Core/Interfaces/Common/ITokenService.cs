using custom_chat_backend.Core.Domain.Entities.User;

namespace custom_chat_backend.Core.Interfaces.Auth;

public interface ITokenService
{
    string GenerateAccessToken(UserEntity user);

    string GenerateRefreshToken();
}