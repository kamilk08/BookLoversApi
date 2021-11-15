namespace BookLovers.Readers.Infrastructure.Persistence
{
    public static class ReadersContextExtensions
    {
        public static void CleanReadersContext(this ReadersContext context) =>
            context.Database.ExecuteSqlCommand(ReadersContextExtensions.CleanReadersContextSqlCommand());

        private static string CleanReadersContextSqlCommand() => @"
    DELETE FROM [ReadersContext].[dbo].[OutboxMessages] 
    DELETE FROM [ReadersContext].[dbo].[InBoxMessages]
    DELETE FROM [ReadersContext].[dbo].[TimeLineActivities]
    DELETE FROM [ReadersContext].[dbo].[ReviewEdits]
    DELETE FROM [ReadersContext].[dbo].[ReviewLikes] 
    DELETE FROM [ReadersContext].[dbo].[Reviews] 
    DELETE FROM [ReadersContext].[dbo].[Authors] 
    DELETE FROM [ReadersContext].[dbo].[Books] 
    DELETE FROM [ReadersContext].[dbo].[AddedResources] 
    DELETE FROM [ReadersContext].[dbo].[ProfileFavourites] 
    DELETE FROM [ReadersContext].[dbo].[Notifications] 
    DELETE FROM [ReadersContext].[dbo].[NotificationWalls]
    DELETE FROM [ReadersContext].[dbo].[Statistics] 
    DELETE FROM [ReadersContext].[dbo].[PrivacyOptions] 
    DELETE FROM [ReadersContext].[dbo].[ProfilePrivacyManagers] 
    DELETE FROM [ReadersContext].[dbo].[Profiles] 
    DELETE FROM [ReadersContext].[dbo].[Readers] 
    DELETE FROM [ReadersContext].[dbo].[TimeLines] 
    DELETE FROM [ReadersContext].[dbo].[Avatars] 
    DELETE FROM [ReadersContext].[dbo].[FavouriteOwners] 
    DELETE FROM [ReadersContext].[dbo].[Favourites] ";
    }
}