namespace custom_chat_backend.Api.Controllers.Demo.GetHelloWorld;

public sealed record GetHelloWorldRequest(string Name)
{
    public const string Route = "/Demo/HelloWorld";
};