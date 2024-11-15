using Flights.Client.Service.Port.FileStorage;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public class JingleFileStorage(IFileStorage fileStorage) : IJingleFileStorage
{
    private const string JingleFolder = "Jingles"; 

    private readonly string[] _allowedFileTypes = {".mp3"};

    public async Task<FileData> StoreJingleFile(IBrowserFile file){

        var fileData = await fileStorage.StoreFile(file, JingleFolder, _allowedFileTypes);
        return fileData;
    }

    public void Delete(string storagePath){
        fileStorage.DeleteFile(storagePath);
    }
}
