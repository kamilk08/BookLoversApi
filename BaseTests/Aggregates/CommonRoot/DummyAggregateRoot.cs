using System;
using BaseTests.Aggregates.DummyEvents;
using BaseTests.Aggregates.Rules;
using BookLovers.Base.Domain.Aggregates;

namespace BaseTests.Aggregates.CommonRoot
{
    public class DummyAggregateRoot : AggregateRoot
    {
        public int Counter { get; private set; }

        public DummyAggregateRoot()
        {
            Id = 1;
            Guid = Guid.NewGuid();
            Status = AggregateStatus.Active.Value;
        }

        public void DoWorkThatProducesEvent()
        {
            CheckBusinessRules(new DummyBusinessRuleThatShouldSucceed());

            ++Counter;

            Events.Add(new DoWorkEvent(Guid, Counter));
        }

        public void DoWorkThatShouldFail()
        {
            CheckBusinessRules(new DummyBusinessRuleThatShouldFail());
        }

        public void DoWorkThatShouldSucceed()
        {
            ++Counter;

            CheckBusinessRules(new DummyBusinessRuleThatShouldSucceed());
        }
    }
}