using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class Followings : ValueObject<Followings>, IStatistic
    {
        public static readonly int Position = StatisticType.Followings.Value;

        public StatisticType Type => StatisticType.Followings;

        public int CurrentValue { get; }

        private Followings()
        {
        }

        private Followings(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new Followings(value);
        }

        public IStatistic Default()
        {
            return new Followings(0);
        }

        public IStatistic Increase()
        {
            return new Followings(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new Followings(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(Followings obj) =>
            CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
    }
}