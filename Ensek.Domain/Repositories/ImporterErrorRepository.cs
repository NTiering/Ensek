using Ensek.Domain.App;
using Ensek.Domain.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ensek.Domain.Repositories;

public class ImporterErrorRepository : IImporterErrorRepository
{
    private readonly DomainDbContext _context;
    private readonly IDateTimeService _dateTimeService;

    public ImporterErrorRepository(DomainDbContext context, IDateTimeService dateTimeService)
    {
        _context = context;
        _dateTimeService = dateTimeService;
    }

    public async Task<IEnumerable<ImporterError>> GetAll(Guid importerId)
    {
        var rtn = await _context.ImporterErrors
            .Where(x => x.ImporterId == importerId)
            .OrderBy(x => x.DataImporterStatus)
            .ThenBy(x => x.Message)
            .ToListAsync();

        return rtn;
    }
    public async Task<ImporterError> Add(ImporterError importerError)
    {
        importerError.CreatedOn = _dateTimeService.UtcNow;
        await _context.AddAsync(importerError);
        await _context.SaveChangesAsync();
        return importerError;
    }
}
