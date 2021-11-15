using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class AuthorBookAddedEventHandler :
        IDomainEventHandler<AuthorBookAdded>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorBookAddedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorBookAdded @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddBookAuthorInternalCommand(@event.BookGuid, @event.AggregateGuid));
        }
    }
}