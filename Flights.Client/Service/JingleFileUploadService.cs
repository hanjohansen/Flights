using Flights.Client.Service.Port;
using Flights.Client.Service.Port.FileStorage;
using Flights.Domain.Exception;
using Flights.Infrastructure.Port;
using Microsoft.AspNetCore.Components.Forms;

namespace Flights.Client.Service;

public class JingleFileUploadService(IJingleFileStorage fileStorage, IPlayerFileRepository playerFileRepo)
    : IJingleFileUploadService
{
    public async Task UploadJingleFile(Guid playerId, IBrowserFile file)
    {
        FileData? fileData;

        try{
            fileData = await fileStorage.StoreJingleFile(file);
        }catch(FlightsGameException fex){
            throw new FlightsGameException("Error saving file: " + fex.Message);
        }catch(Exception){
            throw new FlightsGameException("Error saving file");
        }        

        try{
            await playerFileRepo.TryDeletePlayerJingle(playerId);
            await playerFileRepo.SetPlayerJingle(playerId, fileData.SourceFileName, fileData.StoragePath);
        }catch(FlightsGameException fex){
            fileStorage.Delete(fileData.StoragePath);
            throw new FlightsGameException("Error saving file: " + fex.Message);
        }catch(Exception){
            fileStorage.Delete(fileData.StoragePath);
            throw new FlightsGameException("Error saving file");
        } 
    }

    public async Task ClearPlayerJingle(Guid playerId)
    {
        var jingle = await playerFileRepo.GetPlayerJingle(playerId);
        
        if(jingle == null)
            return;

        try{
            fileStorage.Delete(jingle.StoragePath);
            await playerFileRepo.DeletePlayerJingle(jingle.Id);
        }catch(FlightsGameException fex){
            throw new FlightsGameException("Error removing file: " + fex.Message);
        }catch(Exception){
            throw new FlightsGameException("Error removing file");
        } 
    }
}