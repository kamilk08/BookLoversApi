using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.Publishers;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class PublisherBookAddedEventHandler :
        IDomainEventHandler<PublisherBookAdded>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public PublisherBookAddedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(PublisherBookAdded @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ChangeBookPublisherInternalCommand(@event.BookGuid, @event.AggregateGuid));
        }
    }
}