using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Publication.Infrastructure.Queries.Quotes
{
    public static class QuotesAvailableOrders
    {
        public static readonly IReadOnlyCollection<QuotesOrder> Orders =
            typeof(QuotesOrder).GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(s => s.GetValue(s) as QuotesOrder)
                .ToList();
    }
}