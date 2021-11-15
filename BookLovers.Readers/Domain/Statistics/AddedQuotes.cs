using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class AddedQuotes : ValueObject<AddedQuotes>, IStatistic
    {
        public static int Position => StatisticType.AddedQuotes.Value;

        public StatisticType Type => StatisticType.AddedQuotes;

        public int CurrentValue { get; }

        private AddedQuotes()
        {
        }

        private AddedQuotes(int currentValue) => CurrentValue = currentValue;

        public IStatistic WithValue(int value)
        {
            return new AddedQuotes(value);
        }

        public IStatistic Default()
        {
            return new AddedQuotes(0);
        }

        public IStatistic Increase()
        {
            return new AddedQuotes(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new AddedQuotes(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AddedQuotes obj)
        {
            return CurrentValue == obj.CurrentValue
                   && Type.Equals(obj.Type);
        }
    }
}