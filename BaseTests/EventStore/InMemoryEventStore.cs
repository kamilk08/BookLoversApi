using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;

namespace BaseTests.EventStore
{
    internal class InMemoryEventStore : BaseEventStore, IEventStore
    {
        public InMemoryEventStore(
            IEventStream eventStream,
            IMementoFactory mementoFactory,
            ISnapshotMaker snapshooter,
            ISnapshotProvider snapshotProvider)
            : base(eventStream, mementoFactory, snapshooter, snapshotProvider)
        {
        }

        public async Task StoreEvents<TAggregate>(TAggregate aggregate)
            where TAggregate : class
        {
            var castedAggregate = aggregate as IEventSourcedAggregateRoot;

            if (castedAggregate.GetUncommittedEvents().Count() <= 0)
                return;

            await EventStream.AppendToEventStream(castedAggregate);

            await this.MakeSnapshotAsync(aggregate);
        }

        public async Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class
        {
            var snapshot = await SnapshotProvider.GetSnapshotAsync(aggregateGuid);

            return snapshot != null
                ? await MakeFromSnapshotAsync<TAggregate>(aggregateGuid, snapshot)
                : await MakeFromScratchAsync<TAggregate>(aggregateGuid);
        }
    }
}