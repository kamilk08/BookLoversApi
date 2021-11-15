using System.Collections.Generic;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Base.Domain.Aggregates
{
    public interface IEventSourcedAggregateRoot : IRoot
    {
        AggregateStatus AggregateStatus { get; }

        int Version { get; }

        int LastCommittedVersion { get; }

        void RehydrateAggregate(IList<IEvent> events);

        void ApplySnapshot(IMemento memento);
    }
}