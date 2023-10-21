using Ensek.Domain.Data.Domain;

namespace Ensek.Domain.Repositories;

public interface IAccountUpdateRepository
{
    Task<AccountUpdate> Add(AccountUpdate update);
    Task<bool> Exists(int accountId);
    Task<IEnumerable<AccountUpdate>> GetAll(Guid id);
    Task<bool> SetValidationFlag(Guid id, bool isValid);
}