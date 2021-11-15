using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class BooksInBookcase : ValueObject<BooksInBookcase>, IStatistic
    {
        public static readonly int Position = StatisticType.BooksInBookcase.Value;

        public StatisticType Type => StatisticType.BooksInBookcase;

        public int CurrentValue { get; }

        private BooksInBookcase()
        {
        }

        private BooksInBookcase(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new BooksInBookcase(value);
        }

        public IStatistic Default()
        {
            return new BooksInBookcase(0);
        }

        public IStatistic Increase()
        {
            return new BooksInBookcase(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new BooksInBookcase(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(BooksInBookcase obj)
        {
            return CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
        }
    }
}