using BookLovers.Auth.Domain.Tokens.Services;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Tokens
{
    public class GetRefreshTokenBasedOnProtectedTokenQuery : IQuery<RefreshTokenReadModel>
    {
        public string RefreshToken { get; }

        public RefreshTokenProperties TokenProperties { get; }

        public GetRefreshTokenBasedOnProtectedTokenQuery(
            string refreshToken,
            RefreshTokenProperties tokenProperties)
        {
            RefreshToken = refreshToken;
            TokenProperties = tokenProperties;
        }
    }
}