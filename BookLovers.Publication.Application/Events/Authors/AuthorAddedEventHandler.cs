using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Authors
{
    internal class AuthorAddedEventHandler : IDomainEventHandler<AuthorAdded>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorAddedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorAdded @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddAuthorBookInternalCommand(@event.AuthorGuid, @event.AggregateGuid, true));
        }
    }
}