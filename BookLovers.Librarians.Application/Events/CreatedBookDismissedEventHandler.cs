using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Events.TicketOwners;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.Events
{
    internal class CreatedBookDeclinedEventHandler :
        IDomainEventHandler<CreatedBookDismissed>,
        IDomainEventHandler
    {
        private readonly IInMemoryEventBus _inMemoryEventBus;

        public CreatedBookDeclinedEventHandler(IInMemoryEventBus inMemoryEventBus)
        {
            this._inMemoryEventBus = inMemoryEventBus;
        }

        public Task HandleAsync(CreatedBookDismissed @event)
        {
            var dismissedBy = BookDismissedByLibrarian.Initialize()
                .WithReader(@event.ReaderGuid)
                .DismissedBy(@event.LibrarianGuid)
                .WithBook(@event.BookGuid)
                .WithNotification(@event.Notification);

            return this._inMemoryEventBus.Publish(dismissedBy);
        }
    }
}