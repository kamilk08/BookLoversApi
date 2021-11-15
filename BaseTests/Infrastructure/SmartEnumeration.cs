using BookLovers.Base.Domain.Extensions;

namespace BaseTests.Infrastructure
{
    internal class SmartEnumeration : Enumeration
    {
        internal static readonly SmartEnumeration SmartOne = new SmartEnumeration(1, "SmartOne");
        internal static readonly SmartEnumeration SmartTwo = new SmartEnumeration(2, "SmartTwo");

        protected SmartEnumeration(int value, string name)
        {
        }
    }
}