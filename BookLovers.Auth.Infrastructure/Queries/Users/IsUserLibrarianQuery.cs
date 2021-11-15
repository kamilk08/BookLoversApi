using System;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class IsUserLibrarianQuery : IQuery<bool>
    {
        public Guid UserGuid { get; }

        public IsUserLibrarianQuery(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}