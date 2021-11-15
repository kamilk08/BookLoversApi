using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class IsUserNameUniqueQuery : IQuery<bool>
    {
        public string UserName { get; }

        public IsUserNameUniqueQuery(string userName)
        {
            UserName = userName;
        }
    }
}