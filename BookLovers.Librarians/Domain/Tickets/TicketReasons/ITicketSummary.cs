namespace BookLovers.Librarians.Domain.Tickets.TicketReasons
{
    public interface ITicketSummary
    {
        TicketConcern TicketConcern { get; }

        DecisionMade DecisionMade { get; }

        string Notification { get; }

        bool IsValid();

        void SetNotification(string notification);
    }
}