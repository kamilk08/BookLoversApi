using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public abstract class BaseEventStore
    {
        protected readonly IEventStream EventStream;
        protected readonly ISnapshotMaker SnapshotMaker;
        protected readonly ISnapshotProvider SnapshotProvider;
        protected readonly IMementoFactory MementoFactory;

        public BaseEventStore(
            IEventStream eventStream,
            IMementoFactory mementoFactory,
            ISnapshotMaker snapshooter,
            ISnapshotProvider snapshotProvider)
        {
            EventStream = eventStream;
            MementoFactory = mementoFactory;
            SnapshotMaker = snapshooter;
            SnapshotProvider = snapshotProvider;
        }

        protected async Task MakeSnapshotAsync<TAggregate>(TAggregate aggregate)
            where TAggregate : class
        {
            var snapshot = SnapshotMaker.MakeSnapshot(aggregate);

            if (snapshot != null)
                await SnapshotProvider.SaveSnapshotAsync(snapshot);
        }

        protected async Task<TAggregate> MakeFromScratchAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class
        {
            var aggregateRoot = (TAggregate) ReflectionHelper.CreateInstance(typeof(TAggregate));
            var @events = await EventStream.GetEventStream(aggregateGuid);

            ((IEventSourcedAggregateRoot) aggregateRoot).RehydrateAggregate(
                EventDeserializer.DeserializeToDomainEvents(events));

            return aggregateRoot;
        }

        protected async Task<TAggregate> MakeFromSnapshotAsync<TAggregate>(
            Guid aggregateGuid,
            ISnapshot snapshot)
            where TAggregate : class
        {
            var aggregate = ReflectionHelper.CreateInstance(typeof(TAggregate));
            var memento = MementoFactory.Create<TAggregate>();

            memento = DeserializeMemento<TAggregate>(snapshot.SnapshottedAggregate, memento.GetType());

            ((IEventSourcedAggregateRoot) aggregate).ApplySnapshot(memento);

            aggregate = await RecreateFromMemento((IEventSourcedAggregateRoot) aggregate, snapshot.Version + 1);

            return aggregate as TAggregate;
        }

        private async Task<TAggregate> RecreateFromMemento<TAggregate>(
            TAggregate aggregate,
            int from)
            where TAggregate : IEventSourcedAggregateRoot
        {
            var @events = await EventStream.GetEventStream(
                aggregate.Guid,
                from,
                int.MaxValue);

            ((IEventSourcedAggregateRoot) aggregate).RehydrateAggregate(
                EventDeserializer.DeserializeToDomainEvents(@events));

            return aggregate;
        }

        private IMemento<TAggregate> DeserializeMemento<TAggregate>(
            string snapshottedAggregate,
            Type mementoType)
            where TAggregate : class
        {
            return (IMemento<TAggregate>) JsonConvert.DeserializeObject(
                snapshottedAggregate,
                mementoType,
                SerializerSettings.GetSerializerSettings());
        }
    }
}