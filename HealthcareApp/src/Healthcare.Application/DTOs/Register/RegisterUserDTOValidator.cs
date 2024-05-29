using FluentValidation;
using Domain.Constants;

namespace Application.DTOs.Register;

public class RegisterUserDTOValidator : AbstractValidator<RegisterUserDTO>
{
    public RegisterUserDTOValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First Name cannot be empty.")
            .Matches(RegExConstants.OnlyLettersValidationRegEx).WithMessage("First Name can only contain letters.")
            .MaximumLength(50).WithMessage("First Name cannot be longer than 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last Name cannot be empty.")
            .Matches(RegExConstants.OnlyLettersValidationRegEx).WithMessage("Last Name can only contain letters.")
            .MaximumLength(50).WithMessage("Last Name cannot be longer than 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role cannot be empty.");
    }
}