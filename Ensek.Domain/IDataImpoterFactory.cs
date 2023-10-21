namespace Ensek.Domain;

public interface IDataImpoterFactory
{
    Task<IDataImporter> Build(DataImporterType dataImporterType);
    Task<IDataImporter> Build(Guid dataImporterId);
    Task<IEnumerable<IDataImporter>> BuildAll(DataImporterStatus status);
}