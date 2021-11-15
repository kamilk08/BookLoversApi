using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Domain.Builders
{
    public interface IBuilder
    {
    }

    public interface IBuilder<TAggregate> : IBuilder
        where TAggregate : IRoot
    {
        TAggregate Build();
    }
}