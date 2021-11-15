namespace BookLovers.Publication.Infrastructure.Persistence
{
    public static class PublicationsContextExtensions
    {
        public static void CleanPublicationsContext(this PublicationsContext context)
        {
            context.Database.ExecuteSqlCommand(PublicationsContextExtensions.CleanPublicationsContextSqlCommand());
        }

        private static string CleanPublicationsContextSqlCommand()
        {
            return
                @"DELETE FROM [PublicationsContext].[dbo].[OutboxMessages]   
                    DELETE FROM [PublicationsContext].[dbo].[InBoxMessages]                
                    DELETE FROM [PublicationsContext].[dbo].[Quotes]             
                    DELETE FROM [PublicationsContext].[dbo].[Books]                
                    DELETE FROM [PublicationsContext].[dbo].[Series]                
                    DELETE FROM [PublicationsContext].[dbo].[Cycles]               
                    DELETE FROM [PublicationsContext].[dbo].[Publishers]               
                    DELETE FROM [PublicationsContext].[dbo].[QuoteLikes]             
                    DELETE FROM [PublicationsContext].[dbo].[Authors]              
                    DELETE FROM [PublicationsContext].[dbo].[Readers]";
        }
    }
}