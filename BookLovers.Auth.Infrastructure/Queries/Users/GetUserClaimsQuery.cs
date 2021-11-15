using System.Security.Claims;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class GetUserClaimsQuery : IQuery<ClaimsIdentity>
    {
        public string Email { get; }

        public GetUserClaimsQuery(string email)
        {
            Email = email;
        }
    }
}