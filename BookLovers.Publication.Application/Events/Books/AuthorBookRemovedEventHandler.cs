using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class AuthorBookRemovedEventHandler :
        IDomainEventHandler<AuthorBookRemoved>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorBookRemovedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorBookRemoved @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveBookAuthorInternalCommand(@event.AggregateGuid, @event.BookGuid));
        }
    }
}