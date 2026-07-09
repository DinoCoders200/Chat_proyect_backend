using custom_chat_backend.Core.DTOs.Users.Request;
using custom_chat_backend.Core.DTOs.Users.Response;

namespace custom_chat_backend.Api.Controllers.User.GetUser;

public static class GetUserMapper
{
    public static GetUserInput ToInput(this GetUserRequest req)
    {
        return new GetUserInput(req.IdUser);
    }

    public static GetUserOutput ToOutput(this GetUserOutput req)
    {
        return new GetUserOutput(req.Username, req.Password);
    }
}