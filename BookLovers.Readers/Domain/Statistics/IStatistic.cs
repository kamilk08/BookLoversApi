namespace BookLovers.Readers.Domain.Statistics
{
    public interface IStatistic
    {
        StatisticType Type { get; }

        int CurrentValue { get; }

        IStatistic WithValue(int value);

        IStatistic Default();

        IStatistic Increase();

        IStatistic Decrease();
    }
}