using System;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.PublisherCycles
{
    public class CycleByGuidQuery : IQuery<PublisherCycleDto>
    {
        public Guid PublisherCycleGuid { get; }

        public CycleByGuidQuery(Guid publisherCycleGuid)
        {
            this.PublisherCycleGuid = publisherCycleGuid;
        }
    }
}