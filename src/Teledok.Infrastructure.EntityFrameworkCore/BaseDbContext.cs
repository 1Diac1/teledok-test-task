using Microsoft.EntityFrameworkCore;

namespace Teledok.Infrastructure.EntityFrameworkCore;

public class BaseDbContext<TDbContext> : DbContext
    where TDbContext : DbContext
{
    public BaseDbContext(DbContextOptions<TDbContext> options) 
        : base(options)
    { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}