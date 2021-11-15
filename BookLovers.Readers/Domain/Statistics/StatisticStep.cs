using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.Statistics
{
    public class StatisticStep : Enumeration
    {
        public static readonly StatisticStep Increase = new StatisticStep(1, nameof(Increase));
        public static readonly StatisticStep Decrease = new StatisticStep(2, nameof(Decrease));

        [JsonConstructor]
        protected StatisticStep(byte value, string name)
            : base(value, name)
        {
        }
    }
}