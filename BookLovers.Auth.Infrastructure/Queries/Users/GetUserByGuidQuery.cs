using System;
using BookLovers.Auth.Infrastructure.Dtos.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class GetUserByGuidQuery : IQuery<UserDto>
    {
        public Guid Guid { get; }

        public GetUserByGuidQuery(Guid guid)
        {
            Guid = guid;
        }
    }
}