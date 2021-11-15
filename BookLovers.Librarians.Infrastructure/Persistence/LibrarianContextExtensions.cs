namespace BookLovers.Librarians.Infrastructure.Persistence
{
    public static class LibrarianContextExtensions
    {
        public static void CleanLibrarianContext(this LibrariansContext context) =>
            context.Database.ExecuteSqlCommand(LibrarianContextExtensions.ClearLibrarianContextSqlCommand());

        private static string ClearLibrarianContextSqlCommand() => @"
        DELETE FROM [LibrariansContext].[dbo].[OutboxMessages] 
        DELETE FROM [LibrariansContext].[dbo].[InBoxMessages] 
        DELETE FROM [LibrariansContext].[dbo].[ReviewReportItems] 
        DELETE FROM [LibrariansContext].[dbo].[ReviewReportRegisters]
        DELETE FROM [LibrariansContext].[dbo].[ReviewReportItems] 
        DELETE FROM [LibrariansContext].[dbo].[CreatedTickets] 
        DELETE FROM [LibrariansContext].[dbo].[ResolvedTickets] 
        DELETE FROM [LibrariansContext].[dbo].[Tickets] 
        DELETE FROM [LibrariansContext].[dbo].[TicketOwners] 
        DELETE FROM [LibrariansContext].[dbo].[Librarians] 
        DELETE FROM [LibrariansContext].[dbo].[PromotionWaiters]   ";
    }
}