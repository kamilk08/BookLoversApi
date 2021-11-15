using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Ratings.Application.Commands.Authors;

namespace BookLovers.Ratings.Application.IntegrationEvents.Authors
{
    internal class AuthorAddedToBookHandler :
        IIntegrationEventHandler<AuthorAddedToBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorAddedToBookHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorAddedToBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddAuthorBookInternalCommand(@event.AuthorGuid, @event.BookGuid));
        }
    }
}