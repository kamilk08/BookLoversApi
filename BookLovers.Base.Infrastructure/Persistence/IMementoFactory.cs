using BookLovers.Base.Domain;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface IMementoFactory
    {
        IMemento<TAggregate> Create<TAggregate>()
            where TAggregate : class;
    }
}