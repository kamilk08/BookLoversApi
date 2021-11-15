namespace BookLovers.Auth.Infrastructure.Persistence
{
    public static class AuthContextExtensions
    {
        public static void CleanAuthContext(this AuthContext context) =>
            context.Database.ExecuteSqlCommand(ClearAuthContextSqlCommand());

        private static string ClearAuthContextSqlCommand() => @"
    DELETE FROM [AuthorizationContext].[dbo].[OutboxMessages]
    DELETE FROM [AuthorizationContext].[dbo].[InBoxMessages] 
    DELETE FROM [AuthorizationContext].[dbo].[RegistrationSummaries] 
    DELETE FROM [AuthorizationContext].[dbo].[RefreshTokens] 
    DELETE FROM [AuthorizationContext].[dbo].[Accounts] 
    DELETE FROM [AuthorizationContext].[dbo].[Users]";
    }
}