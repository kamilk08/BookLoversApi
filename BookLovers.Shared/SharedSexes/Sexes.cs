using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Shared.SharedSexes
{
    public static class Sexes
    {
        public static IReadOnlyCollection<Sex> Choices = typeof(Sex)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Select(p => p.GetValue(p) as Sex)
            .ToList();

        public static Sex Get(int sexId) => Choices.SingleOrDefault(p => p.Value == sexId);
        public static bool Has(Sex sex) => Choices.Contains(sex);
        public static bool Has(int sexId) => Get(sexId) != null;
    }
}
