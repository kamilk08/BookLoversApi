namespace BookLovers.Bookcases.Store.Persistence
{
    public static class BookcaseStoreExtensions
    {
        public static void ClearBookcaseStore(this BookcaseStoreContext context)
        {
            context.Database.ExecuteSqlCommand(CleanBookcaseStoreSqlCommand());
        }

        private static string CleanBookcaseStoreSqlCommand()
        {
            return @"
        DELETE FROM [BookcaseStoreContext].[dbo].[Snapshots] 
        DELETE FROM [BookcaseStoreContext].[dbo].[EventEntities]";
        }
    }
}