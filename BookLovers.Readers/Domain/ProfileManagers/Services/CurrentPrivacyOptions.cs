using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions;

namespace BookLovers.Readers.Domain.ProfileManagers.Services
{
    public static class CurrentPrivacyOptions
    {
        public static IDictionary<ProfilePrivacyType, IPrivacyOption>
            CurrentOptions => Assembly.GetExecutingAssembly()
            .GetExportedTypes()
            .Where(p => typeof(IPrivacyOption).IsAssignableFrom(p) && !p.IsInterface)
            .Select(s => ReflectionHelper.CreateInstance(s) as IPrivacyOption)
            .ToDictionary(k => k.PrivacyType, v => v);
    }
}