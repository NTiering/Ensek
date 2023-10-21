using Ensek.Domain.Validators;
using Ensek.Domain.Data.Domain;
using Ensek.Domain.Repositories;
using Ensek.Domain.App;

namespace Ensek.Tests.Domain.Validators
{
    public class GivenAnMeterReadingValidator
    {
     
        static DateTime Today = new(2021, 10, 10);
        static DateTime Yesterday = Today.AddDays(-1);
        static int AccountId = 777;
        static MeterReading ValidMeterReading => new()
        {
            AccountId = AccountId,
            MeterReadingDate = Yesterday,
            LastUpdated = Yesterday,
            Value = AccountId,
            ImporterId = Guid.NewGuid(),
        };

        [Test]
        public void It_can_be_constructed()
        {
            // arrange 
            var repo = A.Fake<IAccountUpdateRepository>();
            var datetime = A.Fake<IDateTimeService>();

            var validator = new MeterReadingValidator(repo, datetime);
           
            // act 

            // assert 
            validator.Should().NotBeNull();
        }

        [Test]
        public async Task Passes_a_valid_reading()
        {
            // arrange 
            var repo = GetAccountRepository();
            var datetime = GetDateTimeService();

            var validator = new MeterReadingValidator(repo, datetime);

            var meterReading = ValidMeterReading;

            // act 
            var result = await validator.ValidateAsync(meterReading);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeTrue();
            result.Errors.Should().HaveCount(0);
        }

        [Test]
        public async Task Unless_accountId_is_missing()
        {
            // arrange 
            var repo = GetAccountRepository();
            var datetime = GetDateTimeService();

            var validator = new MeterReadingValidator(repo, datetime);

            var meterReading = ValidMeterReading;
            meterReading.AccountId = AccountId - 1;
            // act 
            var result = await validator.ValidateAsync(meterReading);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task Unless_MeterReadingDate_is_in_the_future()
        {
            // arrange 
            var repo = GetAccountRepository();
            var datetime = GetDateTimeService();

            var validator = new MeterReadingValidator(repo, datetime);

            var meterReading = ValidMeterReading;
            meterReading.MeterReadingDate = Today.AddDays(1);
            // act 
            var result = await validator.ValidateAsync(meterReading);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        [Test]
        public async Task Unless_Value_is_missing()
        {
            // arrange 
            var repo = GetAccountRepository();
            var datetime = GetDateTimeService();

            var validator = new MeterReadingValidator(repo, datetime);

            var meterReading = ValidMeterReading;
            meterReading.Value = 0;
            // act 
            var result = await validator.ValidateAsync(meterReading);

            // assert 
            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
        }

        private static IDateTimeService GetDateTimeService()
        {
            var datetime = A.Fake<IDateTimeService>();
            A.CallTo(() => datetime.UtcNow).Returns(Today);
            return datetime;
        }

        private static IAccountUpdateRepository GetAccountRepository()
        {
            var repo = A.Fake<IAccountUpdateRepository>();
            A.CallTo(() => repo.Exists(AccountId)).Returns(true);
            return repo;
        }
    }
}