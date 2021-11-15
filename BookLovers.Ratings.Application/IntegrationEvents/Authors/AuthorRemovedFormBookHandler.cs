using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Ratings.Application.Commands.Authors;

namespace BookLovers.Ratings.Application.IntegrationEvents.Authors
{
    internal class AuthorRemovedFormBookHandler :
        IIntegrationEventHandler<AuthorRemovedFromBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorRemovedFormBookHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorRemovedFromBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveAuthorBookInternalCommand(@event.AuthorGuid, @event.BookGuid));
        }
    }
}