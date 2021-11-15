namespace BookLovers.Base.Infrastructure.Services
{
    public interface IAppManager
    {
        string GetConfigValue(string key);

        bool HasKey(string key);
    }
}