using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Ratings.Application.Commands.RatingGivers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Readers
{
    internal class UserBlockedHandler :
        IIntegrationEventHandler<UserBlockedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserBlockedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserBlockedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ArchiveRatingGiverInternalCommand(@event.UserGuid));
        }
    }
}