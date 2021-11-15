using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class IsEmailUniqueQuery : IQuery<bool>
    {
        public string Email { get; }

        public IsEmailUniqueQuery(string email)
        {
            Email = email;
        }
    }
}