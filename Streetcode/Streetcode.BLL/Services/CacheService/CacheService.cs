using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;

namespace Streetcode.BLL.Services.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILoggerService _logger;
        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

        public CacheService(IMemoryCache cache, ILoggerService logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback, TimeSpan cacheDuration)
        {
            _logger.LogInformation(key + "GetOrSetAsync function start!");
            if (_cache.TryGetValue(key, out T cachedItem))
            {
                _logger.LogInformation(key + "TryGetValue function true");
                return cachedItem;
            }

            _logger.LogInformation(key + "TryGetValue function false");
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();

            try
            {
                if (_cache.TryGetValue(key, out cachedItem))
                {
                    _logger.LogInformation(key + "TryGetValue function true");
                    return cachedItem;
                }

                _logger.LogInformation(key + "TryGetValue function false");
                T item = await getItemCallback();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                    SlidingExpiration = cacheDuration,
                    Priority = CacheItemPriority.Normal,
                };

                _cache.Set(key, item, cacheEntryOptions);
                _logger.LogInformation(key + "Set function true");
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
