using System.Runtime.Caching;

namespace ApiCachingApp.Services;

public class CacheService : ICacheService
{

    // do proper exception handling

    // allows connection to in memory cache by ASP.NET
    // Runs as long as app is running 
    private ObjectCache _memoryCache = MemoryCache.Default;

    public T GetData<T>(string key)
    {
        try
        {

            // casting here because we dont want to return an Object, we want that exact type
            T cachedItem = (T)_memoryCache.Get(key);


            if (cachedItem is T) // checking if item is same type as T
            {
                return cachedItem;
            }

            // throw this if it doesnt work, imagine it as an if block
            throw new InvalidCastException($"Cached item is not of type {typeof(T)}.");
        }
        catch (Exception ex)
        {
            // maybe log exception? Idk
            throw;
        }
    }

    public object RemoveData(string key)
    {
        var res = true;

        try
        {
            // check if the key is empty, exit here if the param's empty
            if (!string.IsNullOrEmpty(key))
            {
                var result = _memoryCache.Remove(key);
            }
            res = false;
            return res;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public bool setData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        // res stands for success or failure, like status
        var res = true;

        try
        {
            if(value == null){
                res = false;
                // log it doesnt work and exit
            }

            // check if the key is empty, exit here if the param's empty
            if (!string.IsNullOrEmpty(key))
            {
                _memoryCache.Set(key, value, expirationTime);
            }
            res = false;
            return res;
        }
        catch (Exception ex)
        {
            // maybe log exception? Idk
            throw;
        }
    }
}