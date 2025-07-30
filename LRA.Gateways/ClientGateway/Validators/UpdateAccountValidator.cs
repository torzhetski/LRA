using FluentValidation;
using LRA.Shared.Abstractions.Dtos.Account;

namespace LRA.Gateways.Client.Validators;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountDto>
{
    UpdateAccountValidator()
    {
        RuleFor(x => x.FirstName)
            .MaximumLength(25).WithMessage("First name must not exceed 25 characters.");

        RuleFor(x => x.LastName)
            .MaximumLength(25).WithMessage("Last name must not exceed 25 characters.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+375\d{9}$") 
            .WithMessage("Phone number is invalid.");
    }
}
