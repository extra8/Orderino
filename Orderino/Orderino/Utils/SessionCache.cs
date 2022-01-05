using Microsoft.Extensions.Caching.Memory;
using System;

namespace Orderino.Utils
{
    public class SessionCache
    {
        public MemoryCache MemoryCache { get; set; } = new MemoryCache(new MemoryCacheOptions());

        public void Add<T>(string key, T value)
        {
            MemoryCache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = new TimeSpan(72, 0, 0), AbsoluteExpirationRelativeToNow = new TimeSpan(24, 0, 0) });
        }

        public T Get<T>(string key)
        {
            return MemoryCache.Get<T>(key);
        }
    }
}
