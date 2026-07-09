namespace custom_chat_backend.Api.Controllers.User.GetUser;

public sealed record GetUserRequest(Guid IdUser)
{
    public const string Route = "/User/{IdUser}";
}