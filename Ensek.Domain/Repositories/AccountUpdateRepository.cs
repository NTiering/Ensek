using Ensek.Domain.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Domain.Repositories;


public class AccountUpdateRepository : IAccountUpdateRepository
{
    private readonly DomainDbContext _context;

    public AccountUpdateRepository(DomainDbContext context)
    {
        _context = context;
    }

    public Task<bool> Exists(int accountId)
        => _context.AccountUpdates.AnyAsync(x => x.AccountId == accountId);
    public async Task<AccountUpdate> Add(AccountUpdate update)
    {
        await _context.AddAsync(update);
        await _context.SaveChangesAsync();
        return update;
    }

    public async Task<IEnumerable<AccountUpdate>> GetAll(Guid importerId)
    {
        var accounts = await _context.AccountUpdates
            .Where(x=>x.ImporterId == importerId)
            .ToArrayAsync();
        return accounts;
    }

    public async Task<bool> SetValidationFlag(Guid id, bool isValid)
    {
        var entity = await _context.AccountUpdates.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) { return false; }
        entity.IsValid = isValid;
        await _context.SaveChangesAsync();
        return true;
    }
}
