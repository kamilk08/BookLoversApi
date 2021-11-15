using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookLovers.Base.Domain.Aggregates
{
    public static class AggregateStates
    {
        private static readonly IReadOnlyCollection<AggregateStatus> States =
            typeof(AggregateStatus)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Select(p => p.GetValue(p) as AggregateStatus).ToList();

        public static AggregateStatus Get(int aggregateState) =>
            AggregateStates.States.Single(p => p.Value == aggregateState);
    }
}