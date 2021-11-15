namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface ISnapshotMaker
    {
        ISnapshot MakeSnapshot<TAggregate>(TAggregate aggregate)
            where TAggregate : class;
    }
}