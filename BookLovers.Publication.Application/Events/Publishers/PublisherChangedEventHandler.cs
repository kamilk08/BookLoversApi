using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Publishers
{
    internal class PublisherChangedEventHandler :
        IDomainEventHandler<PublisherChanged>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public PublisherChangedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(PublisherChanged @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new RemovePublisherBookInternalCommand(@event.OldPublisherGuid, @event.AggregateGuid));

            await this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherBookInternalCommand(@event.PublisherGuid, @event.AggregateGuid, true));
        }
    }
}