namespace BookLovers.Base.Infrastructure.Persistence
{
    public class PersistenceSettings
    {
        public SnapshotSettings SnapshotSettings { get; }

        public PersistenceInitialSettings InitialSettings { get; }

        public PersistenceSettings(
            SnapshotSettings snapshotSettings,
            PersistenceInitialSettings initialSettings)
        {
            SnapshotSettings = snapshotSettings;
            InitialSettings = initialSettings;
        }

        public static PersistenceSettings Default()
        {
            return new PersistenceSettings(SnapshotSettings.Default(), PersistenceInitialSettings.Default());
        }

        public static PersistenceSettings SeedWithProcedures()
        {
            return new PersistenceSettings(SnapshotSettings.Default(), PersistenceInitialSettings.SeedWithProcedures());
        }

        public static PersistenceSettings DropAndSeedAgain()
        {
            return new PersistenceSettings(
                SnapshotSettings.Default(),
                PersistenceInitialSettings.DropAndSeedProcedures());
        }

        public static PersistenceSettings DoNotCleanContext()
        {
            return new PersistenceSettings(SnapshotSettings.Default(), PersistenceInitialSettings.LeaveCurrentState());
        }
    }
}