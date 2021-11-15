using System;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Auth.Infrastructure.Queries.Audiences
{
    public class GetAudienceQuery : IQuery<AudienceReadModel>
    {
        public Guid AudienceGuid { get; }

        public GetAudienceQuery(string audienceKey)
        {
            if (!Guid.TryParse(audienceKey, out var result))
                AudienceGuid = Guid.Empty;

            AudienceGuid = result;
        }
    }
}