using FastEndpoints;
using FluentValidation;

namespace custom_chat_backend.Api.Controllers.Auth.Login;

public sealed class LoginValidator
    : Validator<LoginRequest>
{
    public LoginValidator()
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