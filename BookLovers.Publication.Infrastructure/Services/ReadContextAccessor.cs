using System;
using BookLovers.Base.Infrastructure.AppCaching;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class ReadContextAccessor : IReadContextAccessor
    {
        private readonly CacheService _cacher;

        public ReadContextAccessor(CacheService cacher)
        {
            this._cacher = cacher;
        }

        public void AddReadModelId(Guid guid, int readModelId)
        {
            this._cacher.Add(guid.ToString(), readModelId, DateTimeOffset.UtcNow.AddSeconds(30.0));
        }

        public int GetReadModelId(Guid guid)
        {
            return (int) this._cacher.GetValue(guid.ToString());
        }
    }
}