using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Domain.Services
{
    public interface IAggregateManager<TAggregate>
        where TAggregate : IRoot
    {
        void Archive(TAggregate aggregate);
    }
}