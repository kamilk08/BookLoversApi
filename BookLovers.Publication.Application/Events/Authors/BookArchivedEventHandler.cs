using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Authors
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
            var tasks = @event.Authors.Select(async authorGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(
                    new RemoveAuthorBookInternalCommand(authorGuid, @event.AggregateGuid)));

            return Task.WhenAll(tasks);
        }
    }
}