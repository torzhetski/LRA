using FluentValidation;
using LRA.Shared.Abstractions.Models.Auth;

namespace LRA.Gateways.Client.Validators;

public class RegistrationModelValidator : AbstractValidator<RegistrationModel>
{
    public RegistrationModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email required.")
            .EmailAddress().WithMessage("Incorrect email.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password required.")
            .MinimumLength(8).WithMessage("Password must have at least 8 symbols.")
            .Matches(@"[!@#$%^&*(),.?""{}|<>]").WithMessage("Password must contain at least one special character.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("Passwords do not match.");
    }
}
