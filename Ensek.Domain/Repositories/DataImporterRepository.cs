using Ensek.Domain.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Domain.Repositories;

public class DataImporterRepository : IDataImporterRepository
{
    private readonly DomainDbContext _context;

    public DataImporterRepository(DomainDbContext context)
    {
        _context = context;
    }
    public async Task<Importer> Create(DataImporterType type)
    {
        var importer = new Importer
        {
            LastUpdated = DateTime.Now,
            Status = DataImporterStatus.New,
            Type = type
        };

        await _context.AddAsync(importer);
        await _context.SaveChangesAsync();
        return importer;
    }

    public async Task<IEnumerable<Importer>> GetAll(DataImporterStatus status)
    {
        var results = await _context.Importers
            .Where(x => x.Status == status)
            .OrderBy(x =>x.Type)
            .ToArrayAsync();

        return results;

    }

    public async Task<Importer?> Get(Guid id)
    {
        var results = await _context.Importers.FirstOrDefaultAsync(x => x.Id == id);
        return results;
    }

    public async Task<Importer?> UpdateImporterStatus(Guid id, DataImporterStatus status)
    {
        var importer = await _context.Importers.FirstOrDefaultAsync(x => x.Id == id);
        if (importer == null) return null;
        importer.Status = status;
        await _context.SaveChangesAsync();
        return importer;
    }
}
