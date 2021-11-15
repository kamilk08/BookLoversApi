using System;

namespace BookLovers.Base.Infrastructure.Services
{
    public interface IReadContextAccessor
    {
        int GetReadModelId(Guid guid);
    }
}