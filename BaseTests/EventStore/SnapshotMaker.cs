using System;
using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace BaseTests.EventStore
{
    internal class SnapshotMaker : BaseSnapshooter, ISnapshotMaker
    {
        public SnapshotMaker()
            : base(new MementoFactory(), SnapshotSettings.Default())
        {
        }

        public SnapshotMaker(IMementoFactory mementoFactory, SnapshotSettings settings)
            : base(mementoFactory, settings)
        {
        }

        public override ISnapshot MakeSnapshot<TAggregate>(TAggregate aggregate)
        {
            var castedAggregate = aggregate as IEventSourcedAggregateRoot;

            if (castedAggregate == null)
                throw new InvalidCastException(
                    $"Invalid type of aggregate. Aggregate of type ${aggregate.GetType().Name} cannot be snapshotted.");

            if (this.CanMakeSnapshot(
                aggregate.GetType(),
                castedAggregate.Version,
                castedAggregate.GetUncommittedEvents().Count()))
            {
                var memento = MementoFactory.Create<TAggregate>();
                memento.TakeSnapshot(aggregate);

                var serializedMemento =
                    JsonConvert.SerializeObject(memento, SerializerSettings.GetSerializerSettings());

                return new InMemorySnapshot(castedAggregate.Guid, serializedMemento, castedAggregate.Version);
            }

            return default(ISnapshot);
        }
    }
}