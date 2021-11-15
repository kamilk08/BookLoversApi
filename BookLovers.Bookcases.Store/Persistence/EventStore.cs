using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Bookcases.Store.Persistence
{
    public class EventStore : BaseEventStore, IEventStore
    {
        public EventStore(
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

            if (castedAggregate != null && castedAggregate.GetUncommittedEvents().Any())
            {
                await this.MakeSnapshotAsync(aggregate);

                await this.EventStream.AppendToEventStream(castedAggregate);
            }
        }

        public async Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class
        {
            var snapshot = await this.SnapshotProvider.GetSnapshotAsync(aggregateGuid);
            TAggregate aggregate;

            if (snapshot != null)
                aggregate = await this.MakeFromSnapshotAsync<TAggregate>(aggregateGuid, snapshot);
            else
                aggregate = await this.MakeFromScratchAsync<TAggregate>(aggregateGuid);

            return aggregate;
        }
    }
}