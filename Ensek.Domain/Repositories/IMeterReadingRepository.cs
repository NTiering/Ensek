using Ensek.Domain.Data.Domain;

namespace Ensek.Domain.Repositories;

public interface IMeterReadingRepository
{
    Task<MeterReading> Add(MeterReading reading);
    Task<IEnumerable<MeterReading>> GetAll(Guid id);
    Task<bool> SetValidationFlag(Guid id, bool isValid);
}