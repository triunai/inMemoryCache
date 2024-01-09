namespace ApiCachingApp.Services;

public interface ICacheService
{

    /* basically contains 3 methods, 
    one to get, 
    to set and 
    to delete */
    


    // T allows you to put any time of data(Generic)
    T GetData<T>(string key);
    bool setData<T>(string key, T value, DateTimeOffset expirationTime); //<--- DateTimeOffset also includes timezone more precise i guess
    
    object RemoveData(string key);
}
