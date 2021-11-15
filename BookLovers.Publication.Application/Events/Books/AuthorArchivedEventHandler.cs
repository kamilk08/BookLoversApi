using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Application.Events.Books
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
            var tasks = @event.AuthorBooks.Select(async bookGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(
                    new RemoveBookAuthorInternalCommand(@event.AggregateGuid, bookGuid)));

            return Task.WhenAll(tasks);
        }
    }
}