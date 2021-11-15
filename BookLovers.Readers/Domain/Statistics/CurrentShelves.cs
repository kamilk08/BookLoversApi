using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.Statistics
{
    public class CurrentShelves : ValueObject<CurrentShelves>, IStatistic
    {
        public static readonly int DefaultShelvesValue = 3;
        public static readonly int Position = StatisticType.Shelves.Value;

        public int CurrentValue { get; }

        public StatisticType Type => StatisticType.Shelves;

        private CurrentShelves()
        {
        }

        private CurrentShelves(int currentValue)
        {
            CurrentValue = currentValue;
        }

        public IStatistic WithValue(int value)
        {
            return new CurrentShelves(value);
        }

        public IStatistic Default()
        {
            return new CurrentShelves(DefaultShelvesValue);
        }

        public IStatistic Increase()
        {
            return new CurrentShelves(CurrentValue + 1);
        }

        public IStatistic Decrease()
        {
            return new CurrentShelves(CurrentValue - 1);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.CurrentValue.GetHashCode();
            hash = (hash * 23) + this.Type.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(CurrentShelves obj) =>
            CurrentValue == obj.CurrentValue && Type.Equals(obj.Type);
    }
}