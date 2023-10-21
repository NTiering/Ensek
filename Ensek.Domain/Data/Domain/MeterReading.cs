namespace Ensek.Domain.Data.Domain;

public class MeterReading
{
    public Guid Id { get; set; }
    public int AccountId { get; set; }
    public int Value { get; set; }
    public DateTime MeterReadingDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public Guid ImporterId { get; set; }
    public bool? IsValid { get; set; }
}
