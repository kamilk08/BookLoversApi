using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Publishers
{
    internal class BookArchivedEventHandler : IDomainEventHandler<BookArchived>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookArchived @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemovePublisherBookInternalCommand(@event.PublisherGuid, @event.AggregateGuid));
        }
    }
}