using BookLovers.Auth.Infrastructure.Dtos.Tokens;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Tokens
{
    public class GetClaimsIdentityFromProtectedTokenQuery : IQuery<ClaimsIdentityDto>
    {
        public string ProtectedToken { get; }

        public string Issuer { get; }

        public GetClaimsIdentityFromProtectedTokenQuery(string protectedToken, string issuer)
        {
            ProtectedToken = protectedToken;
            Issuer = issuer;
        }
    }
}