using Microsoft.EntityFrameworkCore;

namespace Flights.Infrastructure.Data.EntityMaps;

public static class ModelBuilderExtension
{
    public static void ApplyAppModels(this ModelBuilder modelBuilder)
    {
        TenantMap.Configure(modelBuilder);
        GameMap.Configure(modelBuilder);
        TournamentMaps.Configure(modelBuilder);
    }
}