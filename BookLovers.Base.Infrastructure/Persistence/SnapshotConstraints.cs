namespace BookLovers.Base.Infrastructure.Persistence
{
    public class SnapshotConstraints
    {
        public int MinimalSnapshotFrequency { get; }

        public int MaxSnapshotFrequency { get; }

        private SnapshotConstraints()
        {
        }

        public SnapshotConstraints(int minimalSnapshotFrequency, int maxSnapshotFrequency)
        {
            MinimalSnapshotFrequency = minimalSnapshotFrequency;
            MaxSnapshotFrequency = maxSnapshotFrequency;
        }

        public static SnapshotConstraints DefaultConstraints()
        {
            return new SnapshotConstraints(BaseSnapshooter.MinFrequency, BaseSnapshooter.MaxFrequency);
        }
    }
}