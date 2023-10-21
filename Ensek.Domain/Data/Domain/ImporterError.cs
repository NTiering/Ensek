namespace Ensek.Domain.Data.Domain;

public class ImporterError
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid ImporterId { get; set; }
    public DataImporterStatus DataImporterStatus { get; set; }
    public DateTime CreatedOn { get; internal set; }
}
