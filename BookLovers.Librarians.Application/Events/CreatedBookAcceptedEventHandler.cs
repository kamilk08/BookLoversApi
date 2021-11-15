using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Events.TicketOwners;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.Events
{
    public class CreatedBookAcceptedEventHandler :
        IDomainEventHandler<CreatedBookAccepted>,
        IDomainEventHandler
    {
        private readonly IInMemoryEventBus _eventBus;

        public CreatedBookAcceptedEventHandler(IInMemoryEventBus eventBus)
        {
            this._eventBus = eventBus;
        }

        public Task HandleAsync(CreatedBookAccepted @event)
        {
            var acceptedBy = BookAcceptedByLibrarian
                .Initialize()
                .WithReader(@event.ReaderGuid)
                .WithBook(@event.BookGuid, @event.BookData)
                .WithNotification(@event.Notification)
                .AcceptedBy(@event.LibrarianGuid);

            return this._eventBus.Publish(acceptedBy);
        }
    }
}