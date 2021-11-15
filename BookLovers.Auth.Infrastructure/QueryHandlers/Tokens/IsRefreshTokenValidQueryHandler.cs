using System;
using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Tokens
{
    internal class IsRefreshTokenValidQueryHandler : IQueryHandler<IsRefreshTokenValidQuery, bool>
    {
        private readonly IHashingService _hashingService;

        public IsRefreshTokenValidQueryHandler(IHashingService hashingService)
        {
            _hashingService = hashingService;
        }

        public Task<bool> HandleAsync(IsRefreshTokenValidQuery query)
        {
            var hash = _hashingService
                .GetHash(query.ProtectedToken, query.RefreshToken.Salt);

            return Task.FromResult(IsHashCorrect(query, hash)
                                   && HasBeenRevoked(query, query.RefreshToken.RevokedAt)
                                   && IsExpired(query));
        }

        private bool IsHashCorrect(IsRefreshTokenValidQuery query, string hash)
        {
            return query.RefreshToken.Hash == hash;
        }

        private bool HasBeenRevoked(IsRefreshTokenValidQuery query, DateTime? revokedAt)
        {
            return !query.RefreshToken.RevokedAt.HasValue;
        }

        private bool IsExpired(IsRefreshTokenValidQuery query)
        {
            return DateTime.UtcNow < query.RefreshToken.Expires;
        }
    }
}