using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Base.Domain.Aggregates
{
    public abstract class EventSourcedAggregateRoot : IEventSourcedAggregateRoot, IRoot
    {
        protected IList<IEvent> Events = new List<IEvent>();

        public Guid Guid { get; protected set; }

        public int Version { get; protected set; } = -1;

        public int LastCommittedVersion { get; protected set; } = -1;

        public AggregateStatus AggregateStatus { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedEvents() => this.Events != null
            ? this.Events
            : throw new InvalidOperationException("Cannot return pending events. Events list is null.");

        public void CommitEvents()
        {
            if (this.Events == null)
                throw new InvalidOperationException("Cannot commit events. Events list is null.");

            this.Events.Clear();
        }

        public bool IsActive() => this.AggregateStatus == AggregateStatus.Active;

        public virtual void ApplySnapshot(IMemento memento)
        {
        }

        public void ApplyChange(IEvent @event, bool isNew = true)
        {
            var @eventType = this.GetEventType(@event, out var handlerType);
            this.InvokeHandler(@event, handlerType, @eventType);

            if (isNew && @event.AggregateGuid == this.Guid)
                this.Events.Add(@event);

            ++this.Version;
        }

        public void RehydrateAggregate(IList<IEvent> events)
        {
            foreach (var @event in events)
                this.ApplyChange(@event, false);

            this.LastCommittedVersion = this.Version;
        }

        protected void CheckBusinessRules(IBusinessRule businessRule)
        {
            if (!businessRule.IsFulfilled())
                throw new BusinessRuleNotMetException(businessRule.BrokenRuleMessage);
        }

        private Type GetEventType(IEvent @event, out Type handlerType)
        {
            var type = @event.GetType();
            handlerType = typeof(IHandle<>).MakeGenericType(type);
            return type;
        }

        private void InvokeHandler(IEvent @event, Type handlerType, Type eventType)
        {
            if (!typeof(IHandle<>).MakeGenericType(@event.GetType()).IsAssignableFrom(this.GetType()))
                return;

            handlerType.GetMethod("Handle", new Type[] { eventType })?
                .Invoke(this, new object[] { @event });
        }
    }
}