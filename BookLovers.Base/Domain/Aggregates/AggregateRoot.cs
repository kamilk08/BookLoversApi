using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Base.Domain.Aggregates
{
    public abstract class AggregateRoot : IAggregateRoot, IRoot
    {
        protected readonly List<IEvent> Events = new List<IEvent>();

        public int Id { get; protected set; }

        public Guid Guid { get; protected set; }

        public int Status { get; protected set; }

        protected void CheckBusinessRules(IBusinessRule businessRule)
        {
            if (!businessRule.IsFulfilled())
                throw new BusinessRuleNotMetException(businessRule.BrokenRuleMessage);
        }

        public IEnumerable<IEvent> GetUncommittedEvents() => this.Events.AsEnumerable();

        public void AddEvent(IEvent @event) => this.Events.Add(@event);

        public void CommitEvents() => this.Events.Clear();

        public void ArchiveAggregate()
        {
            if (this.Status != AggregateStatus.Active.Value)
                throw new BusinessRuleNotMetException("Aggregate already archived");

            this.Status = AggregateStatus.Archived.Value;
        }

        public void ActivateAggregate()
        {
            if (this.Status != AggregateStatus.Archived.Value)
                throw new BusinessRuleNotMetException("Aggregate already active");

            this.Status = AggregateStatus.Active.Value;
        }
    }
}