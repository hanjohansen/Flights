using Flights.Client.Service.Port.FileStorage;
using Flights.Domain.Exception;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public class BaseFileStorage(IWebHostEnvironment environment) : IFileStorage
{
    public const int MaxFileSize = 5500000;

    public const string BaseSubFolder = "Storage";

    public async Task<FileData> StoreFile(IBrowserFile file, string subFolder, string[] allowedExt){

        if (file.Size > MaxFileSize)
            throw new FlightsGameException("File size limit (5.5MB)");

        var fileExtension = Path.GetExtension(file.Name);
        if (!allowedExt.Contains(fileExtension))
            throw new FlightsGameException($"File type '{fileExtension}' not allowed");

        var uploadDirectory = BuildBasePath(subFolder);

        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);

        var intFileName = $"{Guid.NewGuid()}{fileExtension}";
        var intPathPhysical = Path.Combine(uploadDirectory, intFileName);
        var intPathRelative = Path.Combine(BaseSubFolder, subFolder, intFileName);

        await using var fs = new FileStream(intPathPhysical, FileMode.Create);
        await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);

        return new FileData(file.Name, intPathRelative);
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
