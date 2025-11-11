using Flights.Domain.Entities;
using Flights.Domain.Exception;
using Flights.Infrastructure.Port;
using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.Repos;

public class TenantRepository(IDbContextFactory<FlightsDbContext> dbFactory) : ITenantRepository
{
    public async Task<TenantEntity> GetTenantByName(string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tenant = await db.Tenants.FirstOrDefaultAsync(x => x.Name == name);

        if (tenant == null)
            throw new FlightsGameException("Tenant not found!");
        
        return tenant;
    }

    public async Task<TenantEntity> GetTenantById(Guid id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tenant = await db.Tenants.FirstOrDefaultAsync(x => x.Id == id);

        if (tenant == null)
            throw new FlightsGameException("Tenant not found!");
        
        return tenant;
    }

    public async Task ChangeTenantName(Guid id, string newName)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tenants = await db.Tenants.ToListAsync();
        var exisiting =  tenants.FirstOrDefault(x => x.Id != id &&  x.Name == newName);
        
        if(exisiting != null)
            throw new FlightsGameException("Tenant name already exists!");
        
        var tenant = tenants.FirstOrDefault(x => x.Id == id);

        if (tenant == null)
            throw new FlightsGameException("Tenant not found!");

        tenant.Name = newName;
        await db.SaveChangesAsync();
    }

    public async Task ChangeTenantPassword(Guid id, string newPassword)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var tenant = await db.Tenants.FirstOrDefaultAsync(x => x.Id == id);

        if (tenant == null)
            throw new FlightsGameException("Tenant not found!");

        tenant.Password = newPassword;
        await db.SaveChangesAsync();
    }
}