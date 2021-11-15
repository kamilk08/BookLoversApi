using System.Linq;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using Newtonsoft.Json;

namespace BookLovers.Readers.Store.Persistence
{
    public class SnapshotMaker : BaseSnapshooter, ISnapshotMaker
    {
        public SnapshotMaker(IMementoFactory mementoFactory)
            : base(mementoFactory, SnapshotSettings.Default())
        {
        }

        public SnapshotMaker(IMementoFactory mementoFactory, SnapshotSettings settings)
            : base(mementoFactory, settings)
        {
        }

        public override ISnapshot MakeSnapshot<TAggregate>(TAggregate aggregate)
        {
            var sourcedAggregateRoot = aggregate as IEventSourcedAggregateRoot;

            if (!CanMakeSnapshot(aggregate.GetType(), sourcedAggregateRoot.Version,
                sourcedAggregateRoot.GetUncommittedEvents().Count()))
                return null;

            var memento = MementoFactory.Create<TAggregate>();
            memento.TakeSnapshot(aggregate);

            var snapshottedAggregate = JsonConvert.SerializeObject(memento, SerializerSettings.GetSerializerSettings());

            return new Snapshot(sourcedAggregateRoot.Guid, sourcedAggregateRoot.Version, snapshottedAggregate);
        }
    }
}