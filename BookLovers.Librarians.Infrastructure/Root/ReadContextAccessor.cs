using BookLovers.Base.Infrastructure.AppCaching;
using BookLovers.Base.Infrastructure.Services;
using System;

namespace BookLovers.Librarians.Infrastructure.Root
{
    internal class ReadContextAccessor : IReadContextAccessor
    {
        private readonly CacheService _cacher;

        public ReadContextAccessor(CacheService cacher)
        {
            _cacher = cacher;
        }

        public void AddReadModelId(Guid guid, int readModelId)
        {
            _cacher.Add(guid.ToString(), readModelId, DateTimeOffset.UtcNow.AddSeconds(30.0));
        }

        public int GetReadModelId(Guid guid)
        {
            return (int) _cacher.GetValue(guid.ToString());
        }
    }
}