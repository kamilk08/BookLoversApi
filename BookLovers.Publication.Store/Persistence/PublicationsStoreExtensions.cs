namespace BookLovers.Publication.Store.Persistence
{
    public static class PublicationsStoreExtensions
    {
        public static void ClearPublicationsStore(this PublicationsStoreContext context)
        {
            context.Database.ExecuteSqlCommand(PublicationsStoreExtensions.CleanPublicationsStoreSqlCommand());
        }

        private static string CleanPublicationsStoreSqlCommand()
        {
            return
                @"DELETE FROM [PublicationsStoreContext].[dbo].[EventEntities] 
          DELETE FROM [PublicationsStoreContext].[dbo].[Snapshots]";
        }
    }
}