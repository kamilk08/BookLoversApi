using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Readers.Store.Persistence
{
    public class EventStore : BaseEventStore, IEventStore
    {
        public EventStore(
            IEventStream eventStream,
            IMementoFactory mementoFactory,
            ISnapshotMaker snapshotMaker,
            ISnapshotProvider snapshotProvider)
            : base(eventStream, mementoFactory, snapshotMaker, snapshotProvider)
        {
        }

        async Task IEventStore.StoreEvents<TAggregate>(TAggregate aggregate)
        {
            var castedAggregate = aggregate as IEventSourcedAggregateRoot;

            if (castedAggregate != null && castedAggregate.GetUncommittedEvents().Any())
            {
                await MakeSnapshotAsync(aggregate);

                await EventStream.AppendToEventStream(castedAggregate);
            }
        }

        public async Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid) where TAggregate : class
        {
            var snapshot = await SnapshotProvider.GetSnapshotAsync(aggregateGuid);
            TAggregate aggregate;

            if (snapshot != null)
                aggregate = await MakeFromSnapshotAsync<TAggregate>(aggregateGuid, snapshot);
            else
                aggregate = await MakeFromScratchAsync<TAggregate>(aggregateGuid);

            return aggregate;
        }
    }
}