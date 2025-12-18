using Flights.Client.Service.Port;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Flights.Client.Service;

public class StorageItem<T>
{
    public StorageItem(T item, TimeSpan? duration = null)
    {
        Item = item;

        Duration = duration;
    }
    public T Item {get;set;}

    public DateTimeOffset Created {get;set;} = DateTimeOffset.UtcNow;

    public TimeSpan? Duration {get; set;}

    public bool HasExpiration()
    {
        return Duration != null;
    }

    public bool IsExpired()
    {
        if(!HasExpiration())
            return false;

        return GetExpiration() < DateTimeOffset.UtcNow;
    }

    public DateTimeOffset GetExpiration()
    {
        return Created + (Duration ?? TimeSpan.FromSeconds(0));
    }

    public void Touch()
    {
        Created = DateTimeOffset.UtcNow;
    }
}

public class BrowserStorage(ProtectedSessionStorage sessionStorage, ProtectedLocalStorage localStorage) : IBrowserStorage
{
    public async Task SetBrowserItem<T>(string key, T item) where T : notnull
    {
        var storageItem = new StorageItem<T>(item);

        await localStorage.SetAsync(key, storageItem);
    }

    public async Task SetBrowserItem<T>(string key, T item, TimeSpan duration) where T : notnull
    {
        var storageItem = new StorageItem<T>(item, duration);

        await localStorage.SetAsync(key, storageItem);
    }

    public async Task<T?> GetBrowserItem<T>(string key)
    {
        try
        {
            var valTask = await localStorage.GetAsync<StorageItem<T>>(key);

            if (valTask.Success && valTask.Value != null)
            {
                if(valTask.Value?.IsExpired() == true)
                {
                    await RemoveBrowserItem(key);
                    return default;
                }

                return valTask.Value!.Item;
            }
        }
        catch (Exception)
        {
            await RemoveBrowserItem(key);
        }

        return default;
    }

    public async Task<T?> GetAndTouchBrowserItem<T>(string key)
    {
        try
        {
            var valTask = await localStorage.GetAsync<StorageItem<T>>(key);

            if (valTask.Success && valTask.Value != null)
            {
                if(valTask.Value?.IsExpired() == true)
                {
                    await RemoveBrowserItem(key);
                    return default;
                }

                var storageItem = valTask.Value!;
                storageItem.Touch();

                 await localStorage.SetAsync(key, storageItem);

                return storageItem.Item;
            }
        }
        catch (Exception)
        {
            await RemoveBrowserItem(key);
        }

        return default;
    }

    public async Task RemoveBrowserItem(string key)
    {
        await localStorage.DeleteAsync(key);
    }

    public async Task SetSessionItem<T>(string key, T item) where T : notnull
    {
        var storageItem = new StorageItem<T>(item);
        await sessionStorage.SetAsync(key, storageItem);
    }

    public async Task SetSessionItem<T>(string key, T item, TimeSpan duration) where T : notnull
    {
        var storageItem = new StorageItem<T>(item, duration);
        await sessionStorage.SetAsync(key, storageItem);
    }

    public async Task<T?> GetSessionItem<T>(string key)
    {
        try
        {
            var valTask = await sessionStorage.GetAsync<StorageItem<T>>(key);

            if (valTask.Success && valTask.Value != null)
            {
                if(valTask.Value?.IsExpired() == true)
                {
                    await RemoveSessionItem(key);
                    return default;
                }

                return valTask.Value!.Item;
            }
        }
        catch (Exception)
        {
            await RemoveSessionItem(key);
        }

        return default;
    }

    public async Task<T?> GetAndTouchSessionItem<T>(string key)
    {
        try
        {
            var valTask = await sessionStorage.GetAsync<StorageItem<T>>(key);

            if (valTask.Success && valTask.Value != null)
            {
                if(valTask.Value?.IsExpired() == true)
                {
                    await RemoveSessionItem(key);
                    return default;
                }

                var storageItem = valTask.Value!;
                storageItem.Touch();

                await sessionStorage.SetAsync(key, storageItem);

                return storageItem.Item;
            }
        }
        catch (Exception)
        {
            await RemoveSessionItem(key);
        }

        return default;
    }

    public async Task RemoveSessionItem(string key)
    {
        await sessionStorage.DeleteAsync(key);
    }
}