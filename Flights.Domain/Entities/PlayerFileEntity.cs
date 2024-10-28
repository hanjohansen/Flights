namespace Flights.Domain.Entities;

public enum PlayerFileType {Jingle, Avatar}

public class PlayerFileEntity : BaseEntity
{
    public Guid PlayerId {get;set;}
    public PlayerEntity Player {get;set;} = null!;

    public PlayerFileType FileType {get;set;}
    public string FileName {get;set;} = null!;
    public string StoragePath {get;set;} = null!;
}