namespace Ensek.Domain.Data.System;

public class Account
{
    public Guid Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public int AccountId { get; set; }

}
