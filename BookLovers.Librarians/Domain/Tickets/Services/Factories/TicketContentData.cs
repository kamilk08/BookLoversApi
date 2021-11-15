namespace BookLovers.Librarians.Domain.Tickets.Services.Factories
{
    public class TicketContentData
    {
        public string Title { get; }

        public string TicketData { get; }

        public TicketContentData(string title, string ticketData)
        {
            this.Title = title;
            this.TicketData = ticketData;
        }
    }
}