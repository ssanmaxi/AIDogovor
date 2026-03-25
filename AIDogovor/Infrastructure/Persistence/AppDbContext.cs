using Microsoft.EntityFrameworkCore;
using AIDogovor.Domain.Entities;
namespace AIDogovor.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Contracts> contracts { get; set; }
    public DbSet<ContractShares> contract_shares { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}