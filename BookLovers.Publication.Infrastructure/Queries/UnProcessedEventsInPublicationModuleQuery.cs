using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Publication.Infrastructure.Queries
{
    public class UnProcessedEventsInPublicationModuleQuery : IQuery<List<IntegrationEvent>>
    {
    }
}