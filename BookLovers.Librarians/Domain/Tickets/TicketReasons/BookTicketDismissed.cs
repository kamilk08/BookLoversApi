namespace BookLovers.Librarians.Domain.Tickets.TicketReasons
{
    public class BookTicketDismissed : TicketSummary, ITicketSummary
    {
        public override TicketConcern TicketConcern => TicketConcern.Book;

        public override DecisionMade DecisionMade => DecisionMade.Dismissed;

        public bool IsValid() => this.IsSummaryComplete();

        public void SetNotification(string notification)
        {
            this.Notification = notification;
        }
    }
}