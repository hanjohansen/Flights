using ATL;
using Flights.Client.Service.Port.FileStorage;
using Flights.Domain.Exception;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public class JingleFileStorage(IFileStorage fileStorage) : IJingleFileStorage
{
    private const string JingleFolder = "Jingles"; 
    
    private const int MaxFileSizeInBytes = 5 * 1024 * 1024;

    public async Task<FileData> StoreJingleFile(IBrowserFile file)
    {
        var fileData = await file.ReadAllBytesAsync(MaxFileSizeInBytes);
        
        Mp3Validator.Validate(file.Name, fileData, MaxFileSizeInBytes);

        var fileInfo = await fileStorage.StoreFile(file.Name, fileData, JingleFolder);
        return fileInfo;
    }

    public void Delete(string storagePath){
        fileStorage.DeleteFile(storagePath);
    }
}

public static class Mp3Validator
{
    public static void Validate(string fileName, byte[] fileData, int maxFileSizeInBytes)
    {
        var extension = Path.GetExtension(fileName).ToLower();

        if (extension != ".mp3")
            throw new FlightsGameException($"Invalid file type '{extension}'");
        
        if(fileData.Length > maxFileSizeInBytes)
            throw new FlightsGameException("File is too large");
        
        if(!IsMp3File(fileData))
            throw new FlightsGameException("File content is not in mp3 format");

        using var ms = new MemoryStream(fileData);
        var track = new Track(ms, "audio/mp3");
        var duration = track.DurationMs;

        if (duration > 10000.0) //10s
            throw new FlightsGameException("Jingle must not exceed 10s in length");
    }
    
    private static bool IsMp3File(byte[] fileBytes)
    {

        var headers = new List<byte[]>
        {
            new byte[] { 49, 44, 33 },
            new byte[] { 73, 68, 51 },
            new byte[] { 0xFF, 0xFB },
            new byte[] { 0xFF, 0xF3 },
            new byte[] { 0xFF, 0xF2 },
        };

        return headers.Any(x => x.SequenceEqual(fileBytes.Take(x.Length)));
    }
}
