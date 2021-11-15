using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class Followers : ValueObject<Followers>, IStatistic
    {
        public static readonly int Position = StatisticType.Followers.Value;

        public StatisticType Type => StatisticType.Followers;

        public int CurrentValue { get; }

        private Followers()
        {
        }

        private Followers(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new Followers(value);
        }

        public IStatistic Default()
        {
            return new Followers(0);
        }

        public IStatistic Increase()
        {
            return new Followers(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new Followers(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(Followers obj) =>
            CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
    }
}