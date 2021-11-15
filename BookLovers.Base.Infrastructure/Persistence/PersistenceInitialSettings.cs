namespace BookLovers.Base.Infrastructure.Persistence
{
    public class PersistenceInitialSettings
    {
        public bool SeedProcedures { get; }

        public bool DropDatabase { get; }

        public bool CleanContext { get; }

        public PersistenceInitialSettings(bool seedProcedures, bool dropDatabase, bool cleanContext)
        {
            SeedProcedures = seedProcedures;
            DropDatabase = dropDatabase;
            CleanContext = cleanContext;
        }

        public static PersistenceInitialSettings Default()
        {
            return new PersistenceInitialSettings(false, false, true);
        }

        public static PersistenceInitialSettings SeedWithProcedures()
        {
            return new PersistenceInitialSettings(true, false, true);
        }

        public static PersistenceInitialSettings DropAndSeedProcedures()
        {
            return new PersistenceInitialSettings(true, true, false);
        }

        public static PersistenceInitialSettings DropDatabases()
        {
            return new PersistenceInitialSettings(false, true, false);
        }

        public static PersistenceInitialSettings LeaveCurrentState()
        {
            return new PersistenceInitialSettings(false, false, false);
        }
    }
}