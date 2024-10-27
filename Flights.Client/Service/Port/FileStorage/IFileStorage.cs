using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.Port.FileStorage;

public record FileData(
    string SourceFileName,
    string StoragePath);

public interface IFileStorage
{
    Task<FileData> StoreFile(IBrowserFile file, string subFolder, string[] allowedExt);

    void DeleteFile(string storagePath);
}