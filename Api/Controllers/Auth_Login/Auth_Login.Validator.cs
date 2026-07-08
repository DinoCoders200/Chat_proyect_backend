using FastEndpoints;
using FluentValidation;

namespace custom_chat_backend.Api.Controllers.Auth_Login;

public sealed class Auth_LoginValidator
    : Validator<Auth_LoginRequest>
{
    public Auth_LoginValidator()
    {
        // El usuario es obligatorio.
        RuleFor(x => x.User)
            .NotEmpty()
            .WithMessage("User is required.");

        // La contraseña es obligatoria.
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}