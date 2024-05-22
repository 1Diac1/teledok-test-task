using Microsoft.EntityFrameworkCore;
using Teledok.Domain.Entities;

namespace Teledok.Infrastructure.EntityFrameworkCore.Postgresql;

public class TeledokDbContext : BaseDbContext<TeledokDbContext>
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Founder> Founders { get; set; }
    
    public TeledokDbContext(DbContextOptions<TeledokDbContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TeledokDbContext).Assembly);
    }
}