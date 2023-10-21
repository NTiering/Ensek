using Ensek.Domain.Data.Domain;

namespace Ensek.Domain.Repositories;

public interface IDataImporterRepository
{
    Task<Importer> Create(DataImporterType type);
    Task<IEnumerable<Importer>> GetAll(DataImporterStatus status);
    Task<Importer?> Get(Guid id);
    Task<Importer?> UpdateImporterStatus(Guid id, DataImporterStatus status);
}