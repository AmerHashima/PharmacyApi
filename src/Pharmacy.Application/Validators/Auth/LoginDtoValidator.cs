using FluentValidation;
using Pharmacy.Application.DTOs.Auth;

namespace Pharmacy.Application.Validators.Auth;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MaximumLength(50)
            .WithMessage("Username cannot exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters");
    }
}