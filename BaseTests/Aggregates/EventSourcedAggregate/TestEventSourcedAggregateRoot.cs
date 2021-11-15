using System;
using BaseTests.Aggregates.DummyEvents;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BaseTests.Aggregates.EventSourcedAggregate
{
    [AllowSnapshot]
    public class TestEventSourcedAggregateRoot :
        EventSourcedAggregateRoot,
        IHandle<DoWorkEvent>,
        IHandle<ClearWorkEvent>,
        IHandle<DummyEventThatChangesStateToArchived>
    {
        public int Counter { get; private set; }

        public TestEventSourcedAggregateRoot()
        {
            Guid = Guid.NewGuid();
            AggregateStatus = AggregateStatus.Active;
        }

        public void DoWork()
        {
            ++Counter;

            ApplyChange(new DoWorkEvent(Guid, Counter));
        }

        public void ClearWork()
        {
            Counter = 0;

            ApplyChange(new ClearWorkEvent(Guid));
        }

        void IHandle<DummyEventThatChangesStateToArchived>.Handle(
            DummyEventThatChangesStateToArchived @event)
        {
            Guid = @event.AggregateGuid;
            AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<DoWorkEvent>.Handle(DoWorkEvent @event)
        {
            Guid = @event.AggregateGuid;
            Counter = @event.Counter;
        }

        void IHandle<ClearWorkEvent>.Handle(ClearWorkEvent @event)
        {
            Guid = @event.AggregateGuid;
            Counter = @event.Counter;
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var aggregateMemento = memento as TestAggregateMemento;

            Counter = aggregateMemento.Counter;
            Guid = aggregateMemento.AggregateGuid;
            Version = aggregateMemento.Version;
            LastCommittedVersion = aggregateMemento.LastCommittedVersion;

            AggregateStatus = AggregateStates.Get(aggregateMemento.AggregateStatus);
        }
    }
}