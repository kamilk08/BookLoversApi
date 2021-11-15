using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using NUnit.Framework;

namespace BaseTests.Aggregates
{
    [TestFixture]
    public abstract class AggregateRootSpecification<TAggregateRoot>
        where TAggregateRoot : IRoot
    {
        protected TAggregateRoot AggregateRoot;
        protected IEnumerable<IEvent> Events;

        protected abstract TAggregateRoot ConfigureAggregate();

        [SetUp]
        public void SetUp()
        {
            AggregateRoot = ConfigureAggregate();
            Events = AggregateRoot.GetUncommittedEvents();
        }
    }
}