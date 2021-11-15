using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class AddedBooks : ValueObject<AddedBooks>, IStatistic
    {
        public static int Position => StatisticType.AddedBooks.Value;

        public StatisticType Type => StatisticType.AddedBooks;

        public int CurrentValue { get; }

        private AddedBooks()
        {
        }

        internal AddedBooks(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new AddedBooks(value);
        }

        public IStatistic Default()
        {
            return new AddedBooks(0);
        }

        public IStatistic Increase()
        {
            return new AddedBooks(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new AddedBooks(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            return 17 + CurrentValue.GetHashCode() + Type.GetHashCode();
        }

        protected override bool EqualsCore(AddedBooks obj)
        {
            return CurrentValue == obj.CurrentValue
                   && Type.Equals(obj.Type);
        }
    }
}