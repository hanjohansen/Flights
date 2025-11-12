using Flights.Domain.Entities;

namespace Flights.Infrastructure.Port;

public interface ITenantRepository
{
    Task<List<Guid>> GetAllTenantIds();
    
    Task<TenantEntity> GetTenantByName(string name);
    
    Task<TenantEntity> GetTenantById(Guid id);
    
    Task ChangeTenantName(Guid id, string newName);
    
    Task ChangeTenantPassword(Guid id, string newPassword);
}