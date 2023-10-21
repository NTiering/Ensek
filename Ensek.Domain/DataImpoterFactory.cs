using Ensek.Domain.App;
using Ensek.Domain.Data.Domain;
using Ensek.Domain.Repositories;
using FluentValidation;

namespace Ensek.Domain;

public class DataImpoterFactory : IDataImpoterFactory
{
    private readonly IDataImporterRepository _dataImporterRepository;
    private readonly IMeterReadingRepository _meterReadingRepository;
    private readonly IAccountUpdateRepository _accountUpdateRepository;
    private readonly IImporterErrorRepository _importerErrorRepository;
    private readonly ISystemRepository _systemRepository;
    private readonly IValidator<MeterReading> _meterReadingValidator;
    private readonly IValidator<AccountUpdate> _accountUpdateValidator;
    private readonly IDateTimeService _dateTimeService;

    public DataImpoterFactory(
        IDataImporterRepository dataImporterRepository,
        IMeterReadingRepository meterReadingRepository,
        IAccountUpdateRepository accountUpdateRepository,
        IImporterErrorRepository importerErrorRepository,
        ISystemRepository systemRepository,
        IValidator<MeterReading> meterReadingValidator,
        IValidator<AccountUpdate> accountUpdateValidator,
        IDateTimeService dateTimeService)
    {
        _dataImporterRepository = dataImporterRepository;
        _meterReadingRepository = meterReadingRepository;
        _accountUpdateRepository = accountUpdateRepository;
        _importerErrorRepository = importerErrorRepository;
        _systemRepository = systemRepository;
        _meterReadingValidator = meterReadingValidator;
        _accountUpdateValidator = accountUpdateValidator;
        _dateTimeService = dateTimeService;
    }

    public async Task<IDataImporter> Build(DataImporterType dataImporterType)
    {
        return dataImporterType switch
        {
            DataImporterType.MeterUpdate => await GetMeterUpdateImporter(),
            DataImporterType.AccountUpdate => await GetAccountUpdateImporter(),
            _ => throw new ArgumentException(null, nameof(dataImporterType)),
        };
    }


    public async Task<IEnumerable<IDataImporter>> BuildAll(DataImporterStatus status)
    {
        var importers = await _dataImporterRepository.GetAll(status);
        var rtn = importers.Select(Hydrate);
        return rtn;
    }

    public async Task<IDataImporter> Build(Guid dataImporterId)
    {
        var importer = await _dataImporterRepository.Get(dataImporterId);
        var rtn = importer != null ?
            Hydrate(importer) :
            null;

        return rtn;
    }
    private async Task<IDataImporter> GetMeterUpdateImporter()
    {
        var importer = await _dataImporterRepository.Create(DataImporterType.MeterUpdate);
        var rtn = Hydrate(importer);
        return rtn;
    }

    private async Task<IDataImporter> GetAccountUpdateImporter()
    {
        var importer = await _dataImporterRepository.Create(DataImporterType.AccountUpdate);
        var rtn = Hydrate(importer);
        return rtn;
    }
    
    private IDataImporter Hydrate(Importer importer)
    {
        if (importer == null) throw new ArgumentNullException(nameof(importer));

        return importer.Type switch
        {
            DataImporterType.MeterUpdate => new MeterUpdateDataImporter(importer,_meterReadingValidator,_meterReadingRepository, _dataImporterRepository,_importerErrorRepository, _systemRepository, _dateTimeService),
            DataImporterType.AccountUpdate => new AccountUpdateDataImporter(importer, _accountUpdateValidator, _accountUpdateRepository, _dataImporterRepository, _importerErrorRepository, _systemRepository, _dateTimeService),
            _ => throw new ArgumentException(null, nameof(importer)),
        };
    }
}
