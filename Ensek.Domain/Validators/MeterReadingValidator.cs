using Ensek.Domain.App;
using Ensek.Domain.Data.Domain;
using Ensek.Domain.Repositories;
using FluentValidation;

namespace Ensek.Domain.Validators;

public class MeterReadingValidator : AbstractValidator<MeterReading>
{
    private readonly IAccountUpdateRepository _accountUpdateRepository;
    private readonly IDateTimeService _dateTimeService;

    public MeterReadingValidator(IAccountUpdateRepository accountUpdateRepository, IDateTimeService dateTimeService)
    {
        _accountUpdateRepository = accountUpdateRepository;
        _dateTimeService = dateTimeService;

        RuleFor(x => x.AccountId).MustAsync(BeValidAccountId).WithMessage("Account id not found");
        RuleFor(x => x.MeterReadingDate).LessThanOrEqualTo(_dateTimeService.UtcNow).WithMessage("MeterReadingDate cannot be in the future");
        RuleFor(x =>x.Value).GreaterThan(0).WithMessage("Value cannont be below 1");
    }

    private async Task<bool> BeValidAccountId(int accountId, CancellationToken token)
    {
        var rtn = await _accountUpdateRepository.Exists(accountId);
        return rtn;
    }
}
