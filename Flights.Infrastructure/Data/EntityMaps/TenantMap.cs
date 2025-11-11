using Flights.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.EntityMaps;

public static class TenantMap
{
    public static void Configure(ModelBuilder builder)
    {
        builder.Entity<TenantEntity>(entity => {
            entity.ToTable("tenants");
            entity.HasKey(x => x.Id);
            
            entity.HasMany(x => x.Games)
                .WithOne(x => x.Tenant)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(x => x.Tournaments)
                .WithOne(x => x.Tenant)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(x => x.Players)
                .WithOne(x => x.Tenant)
                .HasForeignKey(x => x.TenantId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}