using System;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Tokens
{
    public class GetRefreshTokenReadModelQuery : IQuery<RefreshTokenReadModel>
    {
        public Guid TokenGuid { get; }

        public GetRefreshTokenReadModelQuery(Guid tokenGuid)
        {
            TokenGuid = tokenGuid;
        }
    }
}