using System;
using System.Runtime.Caching;

namespace BookLovers.Base.Infrastructure.AppCaching
{
    public class CacheService
    {
        private readonly MemoryCache _cache = MemoryCache.Default;

        public object GetValue(string key) => _cache.Get(key);

        public bool Add(string key, object value, DateTimeOffset timeExpiration)
        {
            if (Contains(key))
                return false;

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = timeExpiration
            };

            return _cache.Add(key, value, policy);
        }

        public bool Delete(string key)
        {
            if (!_cache.Contains(key))
                return false;

            _cache.Remove(key);

            return true;
        }

        public bool Contains(string key) => _cache.Contains(key);
    }
}