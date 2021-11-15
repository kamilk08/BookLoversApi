using System.Configuration;
using System.Linq;
using BookLovers.Base.Infrastructure.Services;

namespace BookLovers.Seed
{
    internal class AppManager : IAppManager
    {
        public string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public bool HasKey(string key)
        {
            return ConfigurationManager.AppSettings.AllKeys
                .Select(k => k.ToUpperInvariant())
                .Contains(key.ToUpperInvariant());
        }
    }
}