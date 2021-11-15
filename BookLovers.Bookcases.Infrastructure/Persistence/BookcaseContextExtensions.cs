namespace BookLovers.Bookcases.Infrastructure.Persistence
{
    public static class BookcaseContextExtensions
    {
        public static void CleanBookcaseContext(this BookcaseContext context)
        {
            context.Database.ExecuteSqlCommand(ClearBookcaseSqlCommand());
        }

        private static string ClearBookcaseSqlCommand()
        {
            return @"
        DELETE FROM [BookcaseContext].[dbo].[InternalCommands] 
        DELETE FROM [BookcaseContext].[dbo].[OutboxMessages] 
        DELETE FROM [BookcaseContext].[dbo].[InBoxMessages] 
        DELETE FROM [BookcaseContext].[dbo].[Books]
        DELETE FROM [BookcaseContext].[dbo].[Bookcases] 
        DELETE FROM [BookcaseContext].[dbo].[Readers] 
        DELETE FROM [BookcaseContext].[dbo].[Shelves] 
        DELETE FROM [BookcaseContext].[dbo].[SettingsManagers] 
        DELETE FROM [BookcaseContext].[dbo].[ShelfRecordTrackers] ";
        }
    }
}