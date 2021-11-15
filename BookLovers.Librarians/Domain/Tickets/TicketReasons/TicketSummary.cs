namespace BookLovers.Librarians.Domain.Tickets.TicketReasons
{
    public abstract class TicketSummary
    {
        public abstract DecisionMade DecisionMade { get; }

        public abstract TicketConcern TicketConcern { get; }

        public virtual string Notification { get; protected set; }

        protected virtual bool IsSummaryComplete() =>
            this.HasNotification() && this.HasDecision() && this.HasReason();

        private bool HasNotification() => this.Notification != string.Empty;

        private bool HasDecision() => this.DecisionMade != null;

        private bool HasReason() => this.TicketConcern != null;
    }
}