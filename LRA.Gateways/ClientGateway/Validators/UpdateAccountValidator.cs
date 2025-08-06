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
            .Matches(@"^\+\d{1,3}\d{6,12}$") 
            .WithMessage("Phone number is invalid.");
    }
}
