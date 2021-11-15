using System.Threading.Tasks;
using BookLovers.Auth.Integration.IntegrationEvents;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Application.Commands.PromotionWaiters;
using BookLovers.Librarians.Application.Commands.TicketOwners;

namespace BookLovers.Librarians.Application.IntegrationEvents
{
    public class UserSignedUpHandler :
        IIntegrationEventHandler<UserSignedUpIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserSignedUpHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(UserSignedUpIntegrationEvent @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new CreateTicketOwnerInternalCommand(@event.ReaderGuid, @event.ReaderId));

            await this._commandDispatcher.SendInternalCommandAsync(
                new CreatePromotionWaiterInternalCommand(@event.ReaderGuid, @event.ReaderId));
        }
    }
}