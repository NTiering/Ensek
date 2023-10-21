using Microsoft.EntityFrameworkCore;
namespace Ensek.Domain.Data.Domain;

public class DomainDbContext : DbContext
{
    public DomainDbContext(DbContextOptions<DomainDbContext> options)
    : base(options)
    {
    }

    public DbSet<Importer> Importers { get; set; }
    public DbSet<MeterReading> MeterReadings { get; set; }
    public DbSet<AccountUpdate> AccountUpdates { get; set; }
    public DbSet<ImporterError> ImporterErrors { get; set; }
}
