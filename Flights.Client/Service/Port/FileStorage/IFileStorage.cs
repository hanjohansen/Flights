using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.Port.FileStorage;

public record FileData(
    string SourceFileName,
    string StoragePath);

public interface IFileStorage
{
    Task<FileData> StoreFile(string fileName, byte[] fileData, string subFolder);

    void DeleteFile(string storagePath);
}