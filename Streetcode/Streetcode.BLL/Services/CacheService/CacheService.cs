using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Streetcode.BLL.Interfaces.Cache;

namespace Streetcode.BLL.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan cacheDuration)
        {
            if (_cache.TryGetValue(key, out T cachedItem))
            {
                return cachedItem;
            }

            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();

            try
            {
                if (_cache.TryGetValue(key, out cachedItem))
                {
                    return cachedItem;
                }

                T item = await getItemCallback();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = cacheDuration,
                    Priority = CacheItemPriority.Normal,
                };

                _cache.Set(key, item, cacheEntryOptions);

                return item;
            }
            finally
            {
                mylock.Release();
            }
        }

        public void RemoveStreetcodeCaches(int streetcodeId)
        {
            string cacheKeyImage = $"ImageCache_{streetcodeId}";
            string cacheKeyText = $"TextCache_{streetcodeId}";

            _cache.Remove(cacheKeyImage);
            _cache.Remove(cacheKeyText);
        }
    }
}
