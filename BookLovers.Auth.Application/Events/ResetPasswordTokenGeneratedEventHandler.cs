using System.Threading.Tasks;
using BookLovers.Auth.Events;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;

namespace BookLovers.Auth.Application.Events
{
    internal class ResetPasswordTokenGeneratedEventHandler :
        IDomainEventHandler<ResetPasswordTokenGenerated>
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ResetPasswordTokenGeneratedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ResetPasswordTokenGenerated @event)
        {
            // SEND USER A PASSWORD RESET TOKEN ON EMAIL THAT HE PROVIDED
            return Task.CompletedTask;
        }
    }
}