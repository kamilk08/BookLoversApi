using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Ratings.Application.Commands.RatingGivers;

namespace BookLovers.Ratings.Application.IntegrationEvents.Readers
{
    internal class SuperAdminCreatedHandler :
        IIntegrationEventHandler<SuperAdminCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SuperAdminCreatedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(SuperAdminCreatedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new CreateRatingGiverInternalCommand(@event.ReaderGuid, @event.ReaderId));
        }
    }
}