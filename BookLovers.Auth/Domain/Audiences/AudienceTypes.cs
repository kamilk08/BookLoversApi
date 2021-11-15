using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Auth.Domain.Audiences
{
    public static class AudienceTypes
    {
        private static readonly IReadOnlyList<AudienceType> _audienceTypes =
            typeof(AudienceType).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as AudienceType).ToList();

        public static AudienceType Get(int audienceId) => _audienceTypes
            .SingleOrDefault(p => p.Value == audienceId);
    }
}