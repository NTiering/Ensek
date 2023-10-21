using Ensek.Domain.Data.Domain;
using FluentValidation;

namespace Ensek.Domain.Validators;

public class AccountUpdateValidator : AbstractValidator<AccountUpdate>
{
    public AccountUpdateValidator()
    {
        RuleFor(x => x.AccountId).GreaterThan(0).WithMessage("Account id must be greater than 0");
        RuleFor(x => x.Surname).NotEmpty().WithMessage("Surname not supplied");
        RuleFor(x => x.Firstname).NotEmpty().WithMessage("Firstname not supplied");
        RuleFor(x => x.ImporterId).NotEqual(Guid.Empty).WithMessage("ImportId not supplied");
    }
}
