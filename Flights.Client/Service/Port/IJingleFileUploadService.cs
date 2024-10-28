using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.Port;

public interface IJingleFileUploadService
{
    Task UploadJingleFile(Guid playerId, IBrowserFile file);

    Task ClearPlayerJingle(Guid playerId);
}