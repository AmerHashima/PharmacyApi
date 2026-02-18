using FluentValidation;
using Pharmacy.Application.Commands.Auth;

namespace Pharmacy.Application.Validators.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.LoginDto)
            .NotNull()
            .WithMessage("Login data is required")
            .SetValidator(new LoginDtoValidator());
    }
}