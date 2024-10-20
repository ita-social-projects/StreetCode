namespace Streetcode.BLL.Interfaces.Cache
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan cacheDuration);
        void RemoveStreetcodeCaches(int streetcodeId);
    }
}
