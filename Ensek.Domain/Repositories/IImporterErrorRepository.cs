using Ensek.Domain.Data.Domain;

namespace Ensek.Domain.Repositories;

public interface IImporterErrorRepository
{
    Task<ImporterError> Add(ImporterError importerError);
    Task<IEnumerable<ImporterError>> GetAll(Guid importerId);
}