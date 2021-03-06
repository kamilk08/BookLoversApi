using System;
using BookLovers.Base.Infrastructure.AppCaching;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Auth.Infrastructure.Persistence
{
    internal class ReadContextAccessor : IReadContextAccessor
    {
        private readonly CacheService _cache;

        public ReadContextAccessor(CacheService cache)
        {
            _cache = cache;
        }

        public void AddReadModelId(Guid guid, int readModelId)
        {
            _cache.Add(guid.ToString(), readModelId, DateTimeOffset.UtcNow.AddSeconds(15.0));
        }

        public int GetReadModelId(Guid guid)
        {
            return (int)_cache.GetValue(guid.ToString());
        }
    }
}