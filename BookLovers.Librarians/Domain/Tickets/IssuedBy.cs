using System;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Librarians.Domain.Tickets
{
    public class IssuedBy : BookLovers.Base.Domain.ValueObject.ValueObject<IssuedBy>
    {
        public int TicketOwnerId { get; private set; }

        public Guid TicketOwnerGuid { get; private set; }

        private IssuedBy()
        {
        }

        public IssuedBy(int ticketOwnerId, Guid ticketOwnerGuid)
        {
            this.TicketOwnerId = ticketOwnerId;
            this.TicketOwnerGuid = ticketOwnerGuid;
        }

        public bool HasOwner() => this.TicketOwnerId != 0 &&
                                  !this.TicketOwnerGuid.IsEmpty();

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.TicketOwnerId.GetHashCode();
            hash = (hash * 23) + this.TicketOwnerGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(IssuedBy obj) =>
            this.TicketOwnerGuid == obj.TicketOwnerGuid
            && this.TicketOwnerId == obj.TicketOwnerId;
    }
}