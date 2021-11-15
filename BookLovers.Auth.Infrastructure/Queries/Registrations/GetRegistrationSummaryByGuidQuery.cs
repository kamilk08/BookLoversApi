using System;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Registrations
{
    public class GetRegistrationSummaryByGuidQuery : IQuery<RegistrationSummaryReadModel>
    {
        public Guid RegistrationSummaryGuid { get; }

        public GetRegistrationSummaryByGuidQuery(Guid registrationSummaryGuid)
        {
            RegistrationSummaryGuid = registrationSummaryGuid;
        }
    }
}