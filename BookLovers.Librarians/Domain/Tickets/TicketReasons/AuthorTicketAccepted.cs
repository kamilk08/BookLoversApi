namespace BookLovers.Librarians.Domain.Tickets.TicketReasons
{
    public class AuthorTicketAccepted : TicketSummary, ITicketSummary
    {
        public override DecisionMade DecisionMade => DecisionMade.Accepted;

        public override TicketConcern TicketConcern => TicketConcern.Author;

        public bool IsValid() => this.IsSummaryComplete();

        public void SetNotification(string notification)
        {
            this.Notification = notification;
        }
    }
}