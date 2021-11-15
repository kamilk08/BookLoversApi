using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookLovers.Auth.Infrastructure.Dtos.Tokens;
using BookLovers.Auth.Infrastructure.Persistence;
using BookLovers.Auth.Infrastructure.Queries.Tokens;
using BookLovers.Auth.Infrastructure.Services;
using BookLovers.Auth.Infrastructure.Services.Tokens;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Auth.Infrastructure.QueryHandlers.Tokens
{
    internal class GetClaimsIdentityFromProtectedTokenHandler :
        IQueryHandler<GetClaimsIdentityFromProtectedTokenQuery, ClaimsIdentityDto>
    {
        private readonly JwtTokenReader _jwtTokenReader;
        private readonly AuthContext _context;
        private readonly IAppManager _appManager;

        public GetClaimsIdentityFromProtectedTokenHandler(
            JwtTokenReader jwtTokenReader,
            AuthContext context,
            IAppManager appManager)
        {
            _jwtTokenReader = jwtTokenReader;
            _context = context;
            _appManager = appManager;
        }

        public Task<ClaimsIdentityDto> HandleAsync(
            GetClaimsIdentityFromProtectedTokenQuery query)
        {
            var keys = _context.Audiences.AsNoTracking()
                .Select(s => s.AudienceGuid.ToString());

            var claimsPrincipal = _jwtTokenReader
                .AddProtectedToken(query.ProtectedToken)
                .AddIssuer(query.Issuer).AddAudiences(keys)
                .AddSigningKey(_appManager.GetConfigValue(JwtSettings.JsonWebTokenKey))
                .ValidateAudience()
                .ValidateIssuer().ReadToken();

            return Task.FromResult(new ClaimsIdentityDto()
            {
                ClaimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity,
                IssuedAtUtc = _jwtTokenReader.IssuedAtUtc,
                ExpiresAtUtc = _jwtTokenReader.ExpiresUtc
            });
        }
    }
}