using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace BookLovers.Base.Infrastructure.Services
{
    public class AppManager : IAppManager
    {
        public NameValueCollection GetSection(string sectionKey)
        {
            return (NameValueCollection) ConfigurationManager.GetSection(sectionKey);
        }

        public string GetConfigValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public bool HasKey(string key)
        {
            return ConfigurationManager.AppSettings
                .AllKeys.Select(k => k.ToUpperInvariant())
                .Contains(key.ToUpperInvariant());
        }
    }
}