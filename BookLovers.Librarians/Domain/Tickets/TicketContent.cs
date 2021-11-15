using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class TicketContent : ValueObject<TicketContent>
    {
        public Guid TicketObjectGuid { get; private set; }

        public string Content { get; private set; }

        public TicketConcern TicketConcern { get; private set; }

        private TicketContent()
        {
        }

        public TicketContent(Guid ticketObjectGuid, string content,
            TicketConcern ticketConcern)
        {
            this.TicketObjectGuid = ticketObjectGuid;
            this.Content = content;
            this.TicketConcern = ticketConcern;
        }

        public bool HasData() => this.Content != string.Empty;

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Content.GetHashCode();
            hash = (hash * 23) + this.TicketConcern.GetHashCode();
            hash = (hash * 23) + this.TicketObjectGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(TicketContent obj)
        {
            return this.TicketObjectGuid == obj.TicketObjectGuid
                   && this.Content == obj.Content &&
                   this.TicketConcern == obj.TicketConcern;
        }
    }
}