namespace BookLovers.Base.Domain.Aggregates
{
    public interface IAggregateRoot : IRoot
    {
        int Status { get; }
    }
}