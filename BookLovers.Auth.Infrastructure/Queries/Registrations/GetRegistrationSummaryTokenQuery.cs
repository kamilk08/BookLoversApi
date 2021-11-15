using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Registrations
{
    public class GetRegistrationSummaryTokenQuery : IQuery<string>
    {
        public string Email { get; }

        public GetRegistrationSummaryTokenQuery(string email)
        {
            Email = email;
        }
    }
}