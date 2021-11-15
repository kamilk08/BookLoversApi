using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Events.TicketOwners;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.Events
{
    internal class CreatedAuthorDismissedEventHandler :
        IDomainEventHandler<CreatedAuthorDismissed>,
        IDomainEventHandler
    {
        private readonly IInMemoryEventBus _eventBus;

        public CreatedAuthorDismissedEventHandler(IInMemoryEventBus eventBus)
        {
            this._eventBus = eventBus;
        }

        public Task HandleAsync(CreatedAuthorDismissed @event)
        {
            var dismissedBy = AuthorDismissedByLibrarian
                .Initialize()
                .WithReader(@event.ReaderGuid)
                .DismissedBy(@event.LibrarianGuid)
                .WithAuthor(@event.AuthorGuid).WithJustification(@event.Notification);

            return this._eventBus.Publish(dismissedBy);
        }
    }
}