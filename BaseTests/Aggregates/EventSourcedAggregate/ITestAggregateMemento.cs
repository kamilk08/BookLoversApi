using BookLovers.Base.Domain;

namespace BaseTests.Aggregates.EventSourcedAggregate
{
    public interface ITestAggregateMemento : IMemento<TestEventSourcedAggregateRoot>, IMemento
    {
        int Counter { get; }
    }
}