using System;
using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Services
{
    public interface IAuthorizeService
    {
        Task<bool> AuthorizeAsync(Guid readerGuid, string role);
    }
}