using Flights.Client.Service.Port.FileStorage;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service.FileStorage;

public class JingleFileStorage : IJingleFileStorage
{
    private readonly IFileStorage _fileStorage;

    private const string JingleFolder = "Jingles"; 

    private readonly string[] _allowedFileTypes = {".mp3"};
    
    public JingleFileStorage(IFileStorage fileStorage){
        _fileStorage = fileStorage;
    }

    public async Task<FileData> StoreJingleFile(IBrowserFile file){

        var fileData = await _fileStorage.StoreFile(file, JingleFolder, _allowedFileTypes);
        return fileData;
    }

    public void Delete(string storagePath){
        _fileStorage.DeleteFile(storagePath);
    }
}
