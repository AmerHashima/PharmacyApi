using FluentValidation;
using Pharmacy.Application.DTOs.Auth;

namespace Pharmacy.Application.Validators.Auth;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MaximumLength(50)
            .WithMessage("Username cannot exceed 50 characters")
            .Matches("^[a-zA-Z0-9._-]+$")
            .WithMessage("Username can only contain letters, numbers, dots, underscores, and hyphens");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("Password must contain at least one lowercase letter, one uppercase letter, and one digit");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Password confirmation is required")
            .Equal(x => x.Password)
            .WithMessage("Password and confirmation password do not match");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Please enter a valid email address")
            .MaximumLength(100)
            .WithMessage("Email cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Mobile)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Please enter a valid mobile number")
            .When(x => !string.IsNullOrEmpty(x.Mobile));

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(50)
            .WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.MiddleName)
            .MaximumLength(50)
            .WithMessage("Middle name cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(50)
            .WithMessage("Last name cannot exceed 50 characters");

        //RuleFor(x => x.Gender)
        //    .Must(BeValidGender)
        //    .WithMessage("Gender must be 'M' for Male, 'F' for Female, or null")
        //    .When(x => x.Gender.HasValue);

        RuleFor(x => x.BirthDate)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Birth date must be in the past")
            .When(x => x.BirthDate.HasValue);

        //RuleFor(x => x.RoleID)
        //    .GreaterThan(0)
        //    .WithMessage("Role ID must be a positive number");
    }

    private static bool BeValidGender(char? gender)
    {
        if (!gender.HasValue) return true;
        return gender == 'M' || gender == 'F' || gender == 'm' || gender == 'f';
    }
}