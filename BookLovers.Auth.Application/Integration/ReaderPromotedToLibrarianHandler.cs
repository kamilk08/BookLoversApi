using System.Threading.Tasks;
using BookLovers.Auth.Application.Commands.Users;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;

namespace BookLovers.Auth.Application.Integration
{
    internal class ReaderPromotedToLibrarianHandler :
        IIntegrationEventHandler<ReaderPromotedToLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderPromotedToLibrarianHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderPromotedToLibrarian @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new PromoteToLibrarianInternalCommand(@event.ReaderGuid));
        }
    }
}