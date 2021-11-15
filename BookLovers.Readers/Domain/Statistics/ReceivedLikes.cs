using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class ReceivedLikes : ValueObject<ReceivedLikes>, IStatistic
    {
        public static readonly int Position = StatisticType.ReceivedLikes.Value;

        public StatisticType Type => StatisticType.ReceivedLikes;

        public int CurrentValue { get; }

        private ReceivedLikes()
        {
        }

        private ReceivedLikes(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new ReceivedLikes(value);
        }

        public IStatistic Default()
        {
            return new ReceivedLikes(0);
        }

        public IStatistic Increase()
        {
            return new ReceivedLikes(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new ReceivedLikes(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ReceivedLikes obj) =>
            CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
    }
}