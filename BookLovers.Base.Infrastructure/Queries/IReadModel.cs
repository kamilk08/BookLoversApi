namespace BookLovers.Base.Infrastructure.Queries
{
    public interface IReadModel
    {
    }

    public interface IReadModel<T> : IReadModel
        where T : IReadModel
    {
        int Id { get; }

        int Status { get; }
    }
}