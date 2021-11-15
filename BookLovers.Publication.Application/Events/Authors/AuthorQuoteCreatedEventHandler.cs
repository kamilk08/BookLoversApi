using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Events.Quotes;

namespace BookLovers.Publication.Application.Events.Authors
{
    internal class AuthorQuoteCreatedEventHandler :
        IDomainEventHandler<AuthorQuoteAdded>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorQuoteCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorQuoteAdded @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddQuoteToAuthorInternalCommand(@event.AggregateGuid, @event.AuthorGuid));
        }
    }
}