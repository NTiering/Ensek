using Ensek.Domain;
using Ensek.Domain.App;
using Ensek.Domain.Data.Domain;
using Ensek.Domain.Repositories;
using FluentValidation;

namespace Ensek.Tests.Domain
{
    public class GivenADataImpoterFactory
    {
        private DataImpoterFactory DataImpoterFactory;

        [SetUp]
        public void Setup()
        {
            var dataImporter = A.Fake<IDataImporterRepository>();
            A.CallTo(() => dataImporter
                .Create(DataImporterType.MeterUpdate))
                .Returns(Task.FromResult(new Importer { Type = DataImporterType.MeterUpdate}));

            A.CallTo(() => dataImporter
                .Create(DataImporterType.AccountUpdate))
                .Returns(Task.FromResult(new Importer { Type = DataImporterType.AccountUpdate }));


            DataImpoterFactory = new DataImpoterFactory
                (dataImporter,
                A.Fake<IMeterReadingRepository>(),
                A.Fake<IAccountUpdateRepository>(),
                A.Fake<IImporterErrorRepository>(),
                A.Fake<ISystemRepository>(),
                A.Fake<IValidator<MeterReading>>(),
                A.Fake<IValidator<AccountUpdate>>(),
                A.Fake<IDateTimeService>()
                );
        }

        [Test]
        public void Can_be_constructed()
        {
            DataImpoterFactory.Should().NotBeNull();
        }

        [Test]
        public async Task Can_Build_MeterUpdate()
        {
            // arrange 
            var importerType = DataImporterType.MeterUpdate;

            // act 
            var result = await DataImpoterFactory.Build(importerType);

            // assert 
            result.ImporterType.Should().Be(importerType);
        }

        [Test]
        public async Task Can_Build_AccountUpdate()
        {
            // arrange 
            var importerType = DataImporterType.AccountUpdate;

            // act 
            var result = await DataImpoterFactory.Build(importerType);

            // assert 
            result.ImporterType.Should().Be(importerType);
        }
    }
}