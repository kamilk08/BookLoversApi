using System;

namespace BookLovers.Base.Infrastructure.Services
{
    public interface IHttpContextAccessor
    {
        Guid UserGuid { get; }

        bool IsAuthenticated { get; }

        bool IsLibrarian();

        bool IsReader();

        bool IsSuperAdmin();
    }
}