namespace Ensek.Domain.Data.Domain;

public class AccountUpdate
{
    public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int AccountId { get; set; }
    public Guid ImporterId { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool? IsValid { get; set; }
}
