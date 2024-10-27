using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.Port.FileStorage;

public interface IJingleFileStorage
{
    Task<FileData> StoreJingleFile(IBrowserFile file);

    void Delete(string jingleFile);
}