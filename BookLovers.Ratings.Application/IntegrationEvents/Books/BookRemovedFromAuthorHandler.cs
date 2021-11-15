using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Ratings.Application.Commands.Authors;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookRemovedFromAuthorHandler :
        IIntegrationEventHandler<AuthorLostBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromAuthorHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorLostBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new RemoveAuthorBookInternalCommand(@event.AuthorGuid, @event.BookGuid));
        }
    }
}