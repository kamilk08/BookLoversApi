using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Ratings.Application.Commands.Authors;

namespace BookLovers.Ratings.Application.IntegrationEvents.Authors
{
    internal class NewAuthorAddedHandler :
        IIntegrationEventHandler<NewAuthorAddedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public NewAuthorAddedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(NewAuthorAddedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new CreateAuthorInternalCommand(@event.AuthorGuid, @event.AuthorId));
        }
    }
}