using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class GivenLikes : ValueObject<GivenLikes>, IStatistic
    {
        public static readonly int Position = StatisticType.GivenLikes.Value;

        public StatisticType Type => StatisticType.GivenLikes;

        public int CurrentValue { get; }

        private GivenLikes()
        {
        }

        private GivenLikes(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new GivenLikes(value);
        }

        public IStatistic Default()
        {
            return new GivenLikes(0);
        }

        public IStatistic Increase()
        {
            return new GivenLikes(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new GivenLikes(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(GivenLikes obj) =>
            CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
    }
}