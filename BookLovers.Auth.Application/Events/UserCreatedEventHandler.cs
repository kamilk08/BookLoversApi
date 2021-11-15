using System;
using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.PasswordResets;
using BookLovers.Auth.Application.Commands.Registrations;
using BookLovers.Auth.Events;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;

namespace BookLovers.Auth.Application.Events
{
    internal class UserCreatedEventHandler : IDomainEventHandler<UserCreated>
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public UserCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(UserCreated @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new CreateRegistrationSummaryInternalCommand(
                    @event.AggregateGuid,
                    @event.Email,
                    Guid.NewGuid().ToString("N")));

            await this._commandDispatcher.SendInternalCommandAsync(
                new CreateTokenPasswordCommand(@event.AggregateGuid, @event.Email));
        }
    }
}