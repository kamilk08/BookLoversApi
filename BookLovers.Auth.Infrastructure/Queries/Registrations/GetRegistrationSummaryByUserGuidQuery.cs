using System;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Registrations
{
    public class GetRegistrationSummaryByUserGuidQuery : IQuery<RegistrationSummaryReadModel>
    {
        public Guid UserGuid { get; }

        public GetRegistrationSummaryByUserGuidQuery(Guid userGuid)
        {
            UserGuid = userGuid;
        }
    }
}