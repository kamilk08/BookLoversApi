using System;
using System.Data.Entity;
using System.Threading.Tasks;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Auth.Infrastructure.Services.Tokens;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Tokens
{
    internal class GetRefreshTokenBasedOnProtectedTokenHandler :
        IQueryHandler<GetRefreshTokenBasedOnProtectedTokenQuery, RefreshTokenReadModel>
    {
        private readonly JwtTokenReader _jwtTokenReader;
        private readonly AuthContext _context;

        public GetRefreshTokenBasedOnProtectedTokenHandler(
            JwtTokenReader jwtTokenReader,
            AuthContext context,
            IHashingService hashingService)
        {
            _jwtTokenReader = jwtTokenReader;
            _context = context;
        }

        public Task<RefreshTokenReadModel> HandleAsync(
            GetRefreshTokenBasedOnProtectedTokenQuery query)
        {
            var claimsPrincipal = _jwtTokenReader
                .AddIssuer(query.TokenProperties.Issuer)
                .AddProtectedToken(query.RefreshToken)
                .AddSigningKey(query.TokenProperties.SigningKey)
                .AddAudience(query.TokenProperties.AudienceGuid.ToString())
                .ReadToken();

            if (claimsPrincipal == null)
                return Task.FromResult<RefreshTokenReadModel>(null);

            if (Guid.TryParse(claimsPrincipal.FindFirst(p => p.Type == "jti").Value, out var tokenGuid))
            {
                var rm = _context.RefreshTokens.AsNoTracking()
                    .SingleOrDefaultAsync(p => p.TokenGuid == tokenGuid);

                return rm;
            }

            return null;
        }
    }
}