using Ensek.Domain.Data.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Ensek.Domain.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly SystemDbContext _context;

        public SystemRepository(SystemDbContext context)
        {
            _context = context;


        }
        public async Task<Account> Add(Account account)
        {
            await _context.AddAsync(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<MeterReading> Add(MeterReading reading)
        {
            await _context.AddAsync(reading);
            await _context.SaveChangesAsync();
            return reading;
        }

        public async Task<IEnumerable<Account>> Search(string search, int pageSize, int pageCount)
        {
            var s = search.ToLower().Trim();
            var ps = Math.Max(pageSize, 5);
            var pc = Math.Max(pageCount, 1);

            var data = await _context
                .Accounts
                .Where(x => x.Surname.ToLower().Contains(s) || x.Firstname.ToLower().Contains(s))
                .OrderBy(x => x.Surname)
                .ThenBy(x => x.Firstname)
                .Take(ps)
                .Skip(ps * (pc-1))
                .ToArrayAsync();            

            return data;
        }

        public async Task<Account?> GetAccount(int accountId)
        { 
            var data = await _context.Accounts.FirstOrDefaultAsync(x=>x.AccountId == accountId);
            return data;
        }

        public async Task<IEnumerable<MeterReading>?> GetReadings(int accountId)
        {
            var data = await _context.MeterReading
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x=>x.MeterReadingDate)
                .ToArrayAsync();
            return data;
        }
    }
   
}
