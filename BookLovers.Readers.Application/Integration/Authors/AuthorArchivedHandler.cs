using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Readers.Application.Commands.Favourites;

namespace BookLovers.Readers.Application.Integration.Authors
{
    internal class AuthorArchivedHandler :
        IIntegrationEventHandler<AuthorArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorArchivedIntegrationEvent @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(new ArchiveFavouriteInternalCommand(@event.AuthorGuid));
        }
    }
}