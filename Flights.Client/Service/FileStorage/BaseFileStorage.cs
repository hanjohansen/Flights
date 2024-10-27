using Flights.Client.Service.Port.FileStorage;
using Flights.Domain.Exceptions;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public class BaseFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _environment;
    public const int MaxFileSize = 5500000;
    
    public BaseFileStorage(IWebHostEnvironment environment){
        _environment = environment;
    }

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
        var intPath = Path.Combine(uploadDirectory, intFileName);

        await using var fs = new FileStream(intPath, FileMode.Create);
        await file.OpenReadStream(MaxFileSize).CopyToAsync(fs);

        return new FileData(file.Name, intPath);
    }

    public void DeleteFile(string storagePath){
        if(!File.Exists(storagePath))
            throw new FlightsGameException("File not found in Storage");

        File.Delete(storagePath);
    }

    private string BuildBasePath(string subFolder){
        var path = Path.Combine(_environment.WebRootPath, "Storage");

        if(!string.IsNullOrEmpty(subFolder))
            path = Path.Combine(path, subFolder);

        return path;
    } 
}
