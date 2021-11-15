using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Publishers
{
    internal class BookCreatedEventHandler : IDomainEventHandler<BookCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookCreated @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherBookInternalCommand(@event.PublisherGuid, @event.AggregateGuid, false));
        }
    }
}