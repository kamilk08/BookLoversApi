using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class CreatedReviews : ValueObject<CreatedReviews>, IStatistic
    {
        public static readonly int Position = StatisticType.Reviews.Value;

        public int CurrentValue { get; }

        public StatisticType Type => StatisticType.Reviews;

        private CreatedReviews()
        {
        }

        private CreatedReviews(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new CreatedReviews(value);
        }

        IStatistic IStatistic.Default()
        {
            return new CreatedReviews(0);
        }

        public IStatistic Increase()
        {
            return new CreatedReviews(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new CreatedReviews(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(CreatedReviews obj)
        {
            return CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
        }
    }
}