using BookLovers.Auth.Application.WriteModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Users
{
    public class IsUserBlockedQuery : IQuery<bool>
    {
        public SignInWriteModel WriteModel { get; }

        public IsUserBlockedQuery(SignInWriteModel writeModel)
        {
            WriteModel = writeModel;
        }
    }
}