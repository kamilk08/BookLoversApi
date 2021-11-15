namespace BookLovers.Ratings.Infrastructure.Persistence
{
    public static class RatingsContextExtensions
    {
        public static void CleanRatingsContext(this RatingsContext context)
        {
            context.Database.ExecuteSqlCommand(RatingsContextExtensions.CleanRatingsContextSqlCommand());
        }

        private static string CleanRatingsContextSqlCommand()
        {
            return
                @"  DELETE FROM [RatingsContext].[dbo].[OutboxMessages]
            DELETE FROM [RatingsContext].[dbo].[InBoxMessages]
            DELETE FROM [RatingsContext].[dbo].[Ratings]
            DELETE FROM [RatingsContext].[dbo].[Books]
            DELETE FROM [RatingsContext].[dbo].[PublisherCycles]
            DELETE FROM [RatingsContext].[dbo].[Publishers] 
            DELETE FROM [RatingsContext].[dbo].[Series] 
            DELETE FROM [RatingsContext].[dbo].[Authors] 
            DELETE FROM [RatingsContext].[dbo].[Readers]";
        }
    }
}