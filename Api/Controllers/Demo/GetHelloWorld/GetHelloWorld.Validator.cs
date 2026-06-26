using FastEndpoints;
using FluentValidation;

namespace custom_chat_backend.Api.Controllers.Demo.GetHelloWorld;

public sealed class GetHelloWorldValidator:Validator<GetHelloWorldRequest>
{
    public  GetHelloWorldValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }
}