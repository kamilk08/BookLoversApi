namespace BookLovers.Readers.Domain.Statistics
{
    public class AddedAuthors : IStatistic
    {
        public static int Position => StatisticType.AddedAuthors.Value;

        public StatisticType Type => StatisticType.AddedAuthors;

        public int CurrentValue { get; }

        private AddedAuthors()
        {
        }

        internal AddedAuthors(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new AddedAuthors(value);
        }

        public IStatistic Default()
        {
            return new AddedAuthors(0);
        }

        public IStatistic Increase()
        {
            return new AddedAuthors(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new AddedAuthors(CurrentValue - 1);
        }
    }
}