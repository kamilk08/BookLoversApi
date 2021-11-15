using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BookLovers.Base.Infrastructure.AppCaching;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Auth.Infrastructure.Services
{
    public class TokenManager : ITokenManager
    {
        private readonly CacheService _cache;

        public TokenManager(CacheService cache) => _cache = cache;

        public async Task AddTokenAsync(Guid tokenGuid, string token) =>
            await Task.Run(() => _cache.Add(tokenGuid.ToString(), token, DateTimeOffset.UtcNow.AddSeconds(30)));

        public async Task DeactivateTokenAsync(Guid tokenGuid) =>
            await Task.Run(() => RemoveToken(tokenGuid));

        public async Task DeactivateCurrentTokenAsync() =>
            await Task.Run(() => DeactivateTokenAsync(Guid.NewGuid()));

        public async Task<bool> IsTokenActiveAsync(Guid tokenGuid)
        {
            var tokenAsync = await GetTokenAsync(tokenGuid);

            return tokenAsync == null;
        }

        public async Task<bool> IsCurrentTokenActiveAsync()
        {
            var flag = await IsTokenActiveAsync(Guid.NewGuid());

            return flag;
        }

        public Task<string> GetTokenAsync(Guid tokenGuid)
        {
            return Task.FromResult(_cache.GetValue(tokenGuid.ToString()) as string);
        }

        public string GetToken(Guid tokenGuid) => _cache.GetValue(tokenGuid.ToString()) as string;

        private string GetCurrentToken(HttpRequest request)
        {
            var header = request.Headers["authorization"];
            string str;
            if (header != null && !header.IsEmpty())
                str = header.Split(' ').Last();
            else
                str = string.Empty;

            return str;
        }

        private void RemoveToken(Guid tokenGuid) =>
            _cache.Delete(tokenGuid.ToString());

        private void BlackListToken(Guid tokenGuid, string token)
        {
            _cache.Add(
                tokenGuid.ToString() + " - blacklisted",
                token,
                new DateTimeOffset(DateTime.UtcNow, TimeSpan.FromMinutes(5)));
        }
    }
}