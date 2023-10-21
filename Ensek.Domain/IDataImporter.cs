using Ensek.Domain.Data.Domain;

namespace Ensek.Domain;


public interface IDataImporter
{
    public Guid Id { get; }
    DataImporterType ImporterType { get; }
    DataImporterStatus Status { get; }
    IEnumerable<ImporterError> Errors { get; }
    Task<(int ItemsRead, int ItemsAccepted)> Load(Stream content);
    Task<(int ItemsRead, int ItemsAccepted)> Validate();
    Task<(int ItemsRead, int ItemsAccepted)> Import();

}
