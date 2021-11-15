using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Quotes
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
            var tasks = @event.Quotes.Select(async quoteGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(new ArchiveQuoteInternalCommand(quoteGuid)));

            return Task.WhenAll(tasks);
        }
    }
}