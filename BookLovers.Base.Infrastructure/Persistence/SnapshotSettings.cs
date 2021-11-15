namespace BookLovers.Base.Infrastructure.Persistence
{
    public class SnapshotSettings
    {
        public int SnapshotFrequency { get; }

        public SnapshotConstraints SnapshotConstraints { get; }

        private SnapshotSettings()
        {
        }

        public SnapshotSettings(int snapshotFrequency, SnapshotConstraints snapshotConstraints)
        {
            SnapshotFrequency = snapshotFrequency;
            SnapshotConstraints = snapshotConstraints;
        }

        public static SnapshotSettings Default() =>
            new SnapshotSettings(
                BaseSnapshooter.MinFrequency,
                new SnapshotConstraints(BaseSnapshooter.MinFrequency, BaseSnapshooter.MaxFrequency));
    }
}