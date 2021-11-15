using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Ratings.Application.Commands.Authors;

namespace BookLovers.Ratings.Application.IntegrationEvents.Books
{
    internal class BookAddedToAuthorHandler :
        IIntegrationEventHandler<AuthorHasNewBookIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookAddedToAuthorHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorHasNewBookIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddAuthorBookInternalCommand(@event.AuthorGuid, @event.BookGuid));
        }
    }
}