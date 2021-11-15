using BaseTests.Aggregates.EventSourcedAggregate;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Persistence;

namespace BaseTests.EventStore
{
    internal class MementoFactory : IMementoFactory
    {
        public IMemento<TAggregate> Create<TAggregate>()
            where TAggregate : class => (IMemento<TAggregate>) new TestAggregateMemento();
    }
}