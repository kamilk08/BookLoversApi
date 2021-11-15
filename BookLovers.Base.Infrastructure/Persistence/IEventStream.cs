using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface IEventStream
    {
        Task<List<IEventEntity>> GetEventStream(
            Guid aggregateGuid,
            int @from,
            int to);

        Task<List<IEventEntity>> GetEventStream(Guid aggregateGuid);

        Task AppendToEventStream<TAggregate>(TAggregate aggregate)
            where TAggregate : IEventSourcedAggregateRoot;
    }
}