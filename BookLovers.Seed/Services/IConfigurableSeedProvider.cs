namespace BookLovers.Seed.Services
{
    public interface IConfigurableSeedProvider<TSeed, TConfiguration> :
        ISeedProvider<TSeed>,
        ISeedProvider
        where TSeed : class
    {
        IConfigurableSeedProvider<TSeed, TConfiguration> SetSeedConfiguration(
            TConfiguration configuration);
    }
}