using FastEndpoints;
using FluentValidation;

namespace custom_chat_backend.Api.Controllers.User.GetUser;

public class GetUserValidator:Validator<GetUserRequest>
{
    public GetUserValidator()
    {
        RuleFor(x=>x.IdUser).NotEmpty().WithMessage("IdUser is required.");
    }
}