namespace BookLovers.Seed.SeedExecutors
{
    public interface ISeedExecutor
    {
        SeedExecutorType ExecutorType { get; }
    }
}