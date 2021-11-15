using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Auth.Events;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;

namespace BookLovers.Auth.Application.Events
{
    internal class RegistrationSummaryCompletedEventHandler :
        IDomainEventHandler<RegistrationSummaryCompleted>
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public RegistrationSummaryCompletedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(RegistrationSummaryCompleted @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new VerifyAccountInternalCommand(@event.UserGuid, @event.CompletedAt));
        }
    }
}