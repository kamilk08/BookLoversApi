namespace BookLovers.Librarians.Domain.Tickets.TicketReasons
{
    public class AuthorTicketDismissed : TicketSummary, ITicketSummary
    {
        public override DecisionMade DecisionMade => DecisionMade.Dismissed;

        public override TicketConcern TicketConcern => TicketConcern.Author;

        public bool IsValid() => this.IsSummaryComplete();

        public void SetNotification(string notification)
        {
            this.Notification = notification;
        }
    }
}