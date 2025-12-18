namespace Flights.Client.Service.Port;

public interface IBrowserStorage
{
    public Task SetBrowserItem<T>(string key, T item) where T : notnull;

    public Task SetBrowserItem<T>(string key, T item, TimeSpan duration) where T : notnull;

    public Task<T?> GetBrowserItem<T>(string key);

    public Task<T?> GetAndTouchBrowserItem<T>(string key);
    
    public Task RemoveBrowserItem(string key);

    public Task SetSessionItem<T>(string key, T item) where T : notnull;
    
    public Task SetSessionItem<T>(string key, T item, TimeSpan duration) where T : notnull;
    
    public Task<T?> GetSessionItem<T>(string key);
    
    public Task<T?> GetAndTouchSessionItem<T>(string key);
    
    public Task RemoveSessionItem(string key);
}