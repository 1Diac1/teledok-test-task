using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teledok.Domain.Entities;

namespace Teledok.Infrastructure.EntityFrameworkCore.Postgresql.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .HasMany(c => c.Founders)
            .WithOne(f => f.Client)
            .HasForeignKey(f => f.ClientId);
    }
}