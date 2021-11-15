using System;
using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Services
{
    public interface ITokenManager
    {
        Task AddTokenAsync(Guid tokenGuid, string token);

        Task DeactivateTokenAsync(Guid tokenGuid);

        Task DeactivateCurrentTokenAsync();

        Task<bool> IsTokenActiveAsync(Guid tokenGuid);

        Task<bool> IsCurrentTokenActiveAsync();

        Task<string> GetTokenAsync(Guid tokenGuid);

        string GetToken(Guid tokenGuid);
    }
}