using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Ratings.Application.Commands.RatingGivers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Readers
{
    internal class UserSignedUpHandler :
        IIntegrationEventHandler<UserSignedUpIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserSignedUpHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(UserSignedUpIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new CreateRatingGiverInternalCommand(@event.ReaderGuid, @event.ReaderId));
        }
    }
}