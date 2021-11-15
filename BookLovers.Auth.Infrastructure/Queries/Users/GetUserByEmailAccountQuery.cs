using BookLovers.Auth.Infrastructure.Dtos.Users;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class GetUserByEmailAccountQuery : IQuery<UserDto>
    {
        public string UserName { get; }

        public GetUserByEmailAccountQuery(string userName)
        {
            UserName = userName;
        }
    }
}