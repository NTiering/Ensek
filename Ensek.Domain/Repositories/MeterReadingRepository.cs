using Ensek.Domain.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Domain.Repositories
{
    public class MeterReadingRepository : IMeterReadingRepository
    {
        private readonly DomainDbContext _context;

        public MeterReadingRepository(DomainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MeterReading>> GetAll(Guid importerId)
        {
            var results = await _context.MeterReadings.Where(x => x.ImporterId == importerId).ToArrayAsync();
            return results;
        }

        public async Task<MeterReading> Add(MeterReading reading)
        {
            await _context.AddAsync(reading);
            await _context.SaveChangesAsync();
            return reading;
        }

        public async Task<bool> SetValidationFlag(Guid id, bool isValid)
        {
            var entity = await _context.MeterReadings.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) { return false; }
            entity.IsValid = isValid;
            await _context.SaveChangesAsync();
            return true;
        }
    }


}
