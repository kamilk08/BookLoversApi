using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Shared.Privacy
{
    public class AvailablePrivacyOptions
    {
        private static readonly IReadOnlyCollection<PrivacyOption> Options =
            typeof(PrivacyOption)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(s => s.GetValue(s) as PrivacyOption).ToList();

        public static PrivacyOption Get(int optionId)
        {
            return Options.SingleOrDefault(p => p.Value == optionId);
        }

        public static bool Has(PrivacyOption privacyOption)
        {
            return Options.Contains(privacyOption);
        }
    }
}