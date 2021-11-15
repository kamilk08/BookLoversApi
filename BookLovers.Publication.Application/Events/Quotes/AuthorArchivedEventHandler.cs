using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Application.Events.Quotes
{
    internal class AuthorArchivedEventHandler :
        IDomainEventHandler<AuthorArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorArchived @event)
        {
            var tasks = @event.AuthorQuotes.Select(async quoteGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(new ArchiveQuoteInternalCommand(quoteGuid)));

            return Task.WhenAll(tasks);
        }
    }
}