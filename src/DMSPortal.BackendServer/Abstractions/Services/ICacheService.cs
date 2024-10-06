namespace DMSPortal.BackendServer.Abstractions.Services;

public interface ICacheService
{
    Task<T> GetData<T>(string key);

    Task<bool> SetData<T>(string key, T value, TimeSpan? expirationTime);

    Task<object> RemoveData(string key);
}