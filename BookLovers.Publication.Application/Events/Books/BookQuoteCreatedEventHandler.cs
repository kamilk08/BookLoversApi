using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.Quotes;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class BookQuoteCreatedEventHandler :
        IDomainEventHandler<BookQuoteAdded>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookQuoteCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookQuoteAdded @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddQuoteToBookInternalCommand(@event.AggregateGuid, @event.BookGuid));
        }
    }
}