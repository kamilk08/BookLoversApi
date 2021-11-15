using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries
{
    public class GetResetPasswordTokenQuery : IQuery<string>
    {
        public string Email { get; }

        public GetResetPasswordTokenQuery(string email)
        {
            Email = email;
        }
    }
}