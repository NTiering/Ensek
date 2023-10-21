namespace Ensek.Domain.Data.Domain;

public class Importer
{
    public Guid Id { get; set; }
    public DataImporterType Type { get; set; }
    public DataImporterStatus Status { get; set; }
    public DateTime LastUpdated { get; set; }
}
