using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Ratings.Application.Commands.Authors;

namespace BookLovers.Ratings.Application.IntegrationEvents.Authors
{
    internal class AuthorArchivedHandler :
        IIntegrationEventHandler<AuthorArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorArchivedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AuthorArchivedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ArchiveAuthorInternalCommand(@event.AuthorGuid));
        }
    }
}