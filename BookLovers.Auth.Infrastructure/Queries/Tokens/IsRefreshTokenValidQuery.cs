using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Tokens
{
    public class IsRefreshTokenValidQuery : IQuery<bool>
    {
        public RefreshTokenReadModel RefreshToken { get; }

        public string ProtectedToken { get; }

        public IsRefreshTokenValidQuery(RefreshTokenReadModel refreshToken, string protectedToken)
        {
            RefreshToken = refreshToken;
            ProtectedToken = protectedToken;
        }
    }
}