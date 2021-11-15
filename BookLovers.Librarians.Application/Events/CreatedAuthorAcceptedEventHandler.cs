using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Librarians.Events.TicketOwners;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.Events
{
    internal class CreatedAuthorAcceptedEventHandler :
        IDomainEventHandler<CreatedAuthorAccepted>
    {
        private readonly IInMemoryEventBus _eventBus;

        public CreatedAuthorAcceptedEventHandler(IInMemoryEventBus eventBus)
        {
            this._eventBus = eventBus;
        }

        public Task HandleAsync(CreatedAuthorAccepted @event)
        {
            var acceptedBy = AuthorAcceptedByLibrarian.Initialize()
                .WithReader(@event.ReaderGuid)
                .AcceptedBy(@event.LibrarianGuid)
                .WithNotification(@event.Justification)
                .WithAuthor(@event.AuthorGuid, @event.AuthorData);

            return this._eventBus.Publish(acceptedBy);
        }
    }
}