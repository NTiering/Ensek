using Microsoft.EntityFrameworkCore;
namespace Ensek.Domain.Data.System;

public class SystemDbContext : DbContext
{
    public SystemDbContext(DbContextOptions<SystemDbContext> options)
    : base(options)
    {
    }

    public DbSet<MeterReading> MeterReading { get; set; }
    public DbSet<Account> Accounts { get; set; }
}
