using FluentValidation;

namespace Application.DTOs.Login;

public class LoginUserDTOValidator : AbstractValidator<LoginUserDTO>
{
    public LoginUserDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}