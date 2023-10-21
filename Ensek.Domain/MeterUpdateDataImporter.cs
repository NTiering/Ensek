using Ensek.Domain.App;
using Ensek.Domain.Data.Domain;
using Ensek.Domain.Repositories;
using FluentValidation;
using System.Data;
using System.Text;

namespace Ensek.Domain;

public class MeterUpdateDataImporter : IDataImporter
{
    private readonly Importer _importer;
    private readonly IValidator<MeterReading> _validator;
    private readonly IMeterReadingRepository _meterReadingRepository;
    private readonly IDataImporterRepository _dataImporterRepository;
    private readonly IImporterErrorRepository _importerErrorRepository;
    private readonly ISystemRepository _systemRepository;
    private readonly IDateTimeService _dateTimeService;

    public Guid Id => _importer.Id;
    public DateTime LastUpdated => _importer.LastUpdated;
    public DataImporterStatus Status => _importer.Status;
    public DataImporterType ImporterType => DataImporterType.MeterUpdate;
    public IEnumerable<ImporterError> Errors => GetErrors();        

    public MeterUpdateDataImporter(
        Importer importer, 
        IValidator<MeterReading> validator,
        IMeterReadingRepository meterReadingRepository, 
        IDataImporterRepository dataImporterRepository,
        IImporterErrorRepository importerErrorRepository,
        ISystemRepository systemRepository,
        IDateTimeService dateTimeService)
    {
        _importer = importer;
        _validator = validator;
        _meterReadingRepository = meterReadingRepository;
        _dataImporterRepository = dataImporterRepository;
        _importerErrorRepository = importerErrorRepository;
        _systemRepository = systemRepository;
        _dateTimeService = dateTimeService;
    }

    public async Task<(int ItemsRead, int ItemsAccepted)> Load(Stream stream)
    {
        await UpdateImporterStatus(DataImporterStatus.Loading);
        int itemsRead = 0;
        int itemsAccepted = 0;

        stream.Position = 0;
        using StreamReader reader = new(stream, Encoding.UTF8);
        var txt = reader.ReadToEnd();

        var lines = txt.Split('\n');
        foreach (var line in lines)
        {
            itemsRead++;
            var reading = MakeMeterReading(line);
            if (reading == null)
            {
                await _importerErrorRepository.Add(new ImporterError
                {
                    DataImporterStatus = _importer.Status,
                    ImporterId = _importer.Id,
                    Message = line                      
                });

                continue;
            }

            itemsAccepted++;
            await _meterReadingRepository.Add(reading);
        }
        await UpdateImporterStatus(DataImporterStatus.Loaded);
        return (itemsRead, itemsAccepted);
    }        

    public async Task<(int ItemsRead, int ItemsAccepted)> Validate()
    {
        await UpdateImporterStatus(DataImporterStatus.Validating);
        int itemsRead = 0;
        int itemsAccepted = 0;

        var readings = await _meterReadingRepository.GetAll(_importer.Id);
        
        foreach (var reading in readings) 
        {
            itemsRead++;
            var validationResults = await _validator.ValidateAsync(reading);                

            var setValidationFlag = await _meterReadingRepository.SetValidationFlag(reading.Id, validationResults.IsValid);

            if (setValidationFlag == false) throw new InvalidOperationException($"Unable to set validation on {reading.Id}");

            if (validationResults.IsValid)
            {
                itemsAccepted++;
                continue;
            } 
            foreach (var errorMessages in validationResults.Errors.Select(x => x.ErrorMessage))
            {
                await _importerErrorRepository.Add(new ImporterError
                {
                    CreatedOn = _dateTimeService.UtcNow,
                    DataImporterStatus = _importer.Status,
                    ImporterId = _importer.Id,
                    Message = errorMessages ?? "Validation Failed"
                });
            }
        }

        await UpdateImporterStatus(DataImporterStatus.Validated);
        return (itemsRead, itemsAccepted);
    }

    public async Task<(int ItemsRead, int ItemsAccepted)> Import()
    {
        await UpdateImporterStatus(DataImporterStatus.Importing);
        int itemsRead = 0;
        int itemsAccepted = 0;

        var items = (await _meterReadingRepository.GetAll(Id)).Where(x=>x.IsValid.HasValue && x.IsValid.Value);

        foreach(var item in items) 
        {
            itemsRead++;
            await _systemRepository.Add(new Data.System.MeterReading {
                AccountId = item.AccountId,
                Value = item.Value,
                MeterReadingDate = item.MeterReadingDate });
            itemsAccepted++;
        }


        await UpdateImporterStatus(DataImporterStatus.Imported);
        return (itemsRead, itemsAccepted);
    }

    private async Task UpdateImporterStatus(DataImporterStatus status)
    {
        await _dataImporterRepository.UpdateImporterStatus(_importer.Id, status);
    }

    private IEnumerable<ImporterError> GetErrors()
    {
        var errors = _importerErrorRepository.GetAll(_importer.Id).GetAwaiter().GetResult();
        return errors;
    }

    private MeterReading? MakeMeterReading(string line)
    {
        if (string.IsNullOrEmpty(line)) return null;
        var parts = line.Split(",").Where(x => string.IsNullOrEmpty(x) == false).ToArray();
        if (parts.Length != 3) return null;
        if (int.TryParse(parts[2], out int readingValue) == false) return null;
        if (int.TryParse(parts[0], out int accountId) == false) return null;
        if (DateTime.TryParse(parts[1], out DateTime readingDate) == false) return null;
        var reading = new MeterReading
        {
            Value = readingValue,
            AccountId = accountId,
            MeterReadingDate = readingDate,
            LastUpdated = _dateTimeService.UtcNow,
            ImporterId = Id
        };
        return reading;

    }


}
