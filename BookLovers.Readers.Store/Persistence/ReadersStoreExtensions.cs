namespace BookLovers.Readers.Store.Persistence
{
    public static class ReadersStoreExtensions
    {
        public static void CleanReadersStore(this ReadersStoreContext context)
        {
            context.Database.ExecuteSqlCommand(CleanReadersStoreSqlCommand());
        }

        private static string CleanReadersStoreSqlCommand()
        {
            return @"DELETE FROM [ReadersStoreContext].[dbo].[EventEntities]
                     DELETE FROM [ReadersStoreContext].[dbo].[Snapshots]";
        }
    }
}