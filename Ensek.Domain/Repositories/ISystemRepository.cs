using Ensek.Domain.Data.System;

namespace Ensek.Domain.Repositories;

public interface ISystemRepository
{
    Task<Account> Add(Account account);
    Task<MeterReading> Add(MeterReading reading);
    Task<IEnumerable<Account>> Search(string search, int pageSize, int pageCount);
    Task<Account?> GetAccount(int accountId);
    Task<IEnumerable<MeterReading>?> GetReadings(int accountId);
}