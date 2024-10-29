using Flights.Client.Service.Port;
using Flights.Client.Service.Port.FileStorage;
using Flights.Domain.Exceptions;
using Flights.Infrastructure.Port;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service;

public class JingleFileUploadService : IJingleFileUploadService     
{
    private readonly IJingleFileStorage _fileStorage;
    private readonly IPlayerFileRepository _playerFileRepo;

    public JingleFileUploadService(IJingleFileStorage fileStorage, IPlayerFileRepository playerFileRepo){
            _fileStorage = fileStorage;
            _playerFileRepo = playerFileRepo;
    }

    public async Task UploadJingleFile(Guid playerId, IBrowserFile file)
    {
        FileData? fileData;

        try{
            fileData = await _fileStorage.StoreJingleFile(file);
        }catch(FlightsGameException fex){
            throw new FlightsGameException("Error saving file: " + fex.Message);
        }catch(Exception){
            throw new FlightsGameException("Error saving file");
        }        

        try{
            await _playerFileRepo.TryDeletePlayerJingle(playerId);
            await _playerFileRepo.SetPlayerJingle(playerId, fileData.SourceFileName, fileData.StoragePath);
        }catch(FlightsGameException fex){
            _fileStorage.Delete(fileData.StoragePath);
            throw new FlightsGameException("Error saving file: " + fex.Message);
        }catch(Exception){
            _fileStorage.Delete(fileData.StoragePath);
            throw new FlightsGameException("Error saving file");
        } 
    }

    public async Task ClearPlayerJingle(Guid playerId)
    {
        var jingle = await _playerFileRepo.GetPlayerJingle(playerId);
        
        if(jingle == null)
            return;

        try{
            _fileStorage.Delete(jingle.StoragePath);
            await _playerFileRepo.DeletePlayerJingle(jingle.Id);
        }catch(FlightsGameException fex){
            throw new FlightsGameException("Error removing file: " + fex.Message);
        }catch(Exception){
            throw new FlightsGameException("Error removing file");
        } 
    }
}