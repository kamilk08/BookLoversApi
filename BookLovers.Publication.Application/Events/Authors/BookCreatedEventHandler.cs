using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Authors
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
            var tasks = @event.BookAuthors.Select(async authorGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(
                    new AddAuthorBookInternalCommand(authorGuid, @event.AggregateGuid, false)));

            return Task.WhenAll(tasks);
        }
    }
}