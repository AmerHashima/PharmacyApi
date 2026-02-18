using FluentValidation;
using Pharmacy.Application.Commands.Auth;

namespace Pharmacy.Application.Validators.Auth;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.RegisterDto)
            .NotNull()
            .WithMessage("Registration data is required")
            .SetValidator(new RegisterDtoValidator());
    }
}