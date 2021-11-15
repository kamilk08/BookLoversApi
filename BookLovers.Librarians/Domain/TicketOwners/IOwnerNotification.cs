using System;
using BookLovers.Base.Domain.Events;
using BookLovers.Librarians.Domain.Tickets;

namespace BookLovers.Librarians.Domain.TicketOwners
{
    public interface IOwnerNotification : IEvent
    {
        Guid TicketGuid { get; }

        Guid ReaderGuid { get; }

        TicketConcern TicketConcern { get; }
    }
}