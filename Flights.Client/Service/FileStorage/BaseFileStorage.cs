using Flights.Client.Service.Port.FileStorage;
using Flights.Domain.Exception;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public class BaseFileStorage(IWebHostEnvironment environment) : IFileStorage
{
    public const string BaseSubFolder = "Storage";

    public async Task<FileData> StoreFile(string fileName, byte[] fileData, string subFolder){
        var uploadDirectory = BuildBasePath(subFolder);

        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);

        var fileExtension = Path.GetExtension(fileName);
        var intFileName = $"{Guid.NewGuid()}{fileExtension}";
        var intPathPhysical = Path.Combine(uploadDirectory, intFileName);
        var intPathRelative = Path.Combine(BaseSubFolder, subFolder, intFileName);

        await File.WriteAllBytesAsync(intPathPhysical, fileData);

        return new FileData(fileName, intPathRelative);
    }

    public void DeleteFile(string storagePath){
        var path = Path.Combine(environment.WebRootPath, storagePath);

        if(!File.Exists(path))
            throw new FlightsGameException("File not found in Storage");

        File.Delete(path);
    }

    private string BuildBasePath(string subFolder){
        var path = Path.Combine(environment.WebRootPath, BaseSubFolder);

        if(!string.IsNullOrEmpty(subFolder))
            path = Path.Combine(path, subFolder);

        return path;
    } 
}
