namespace Ensek.Domain.Data.System;

public class MeterReading
{
    public Guid Id { get; set; }
    public int AccountId { get; set; }
    public int Value { get; set; }
    public DateTime MeterReadingDate { get; set; }      
}
