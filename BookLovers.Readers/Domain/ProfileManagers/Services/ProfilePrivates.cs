using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Readers.Domain.ProfileManagers.Services
{
    public static class ProfilePrivates
    {
        private static IReadOnlyList<ProfilePrivacyType> AvailableOptions =>
            typeof(ProfilePrivacyType)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as ProfilePrivacyType)
                .ToList();

        public static ProfilePrivacyType Get(int optionId)
            => AvailableOptions.SingleOrDefault(p => p.Value == optionId);

        public static bool Has(int optionId) => Get(optionId) != null;
    }
}