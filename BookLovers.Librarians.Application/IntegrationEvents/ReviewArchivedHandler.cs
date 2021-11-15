using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Application.Commands.ReviewRegisters;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.IntegrationEvents
{
    internal class ReviewArchivedHandler :
        IIntegrationEventHandler<ReviewArchivedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewArchivedHandler(IInternalCommandDispatcher commandDispatcher) =>
            this._commandDispatcher = commandDispatcher;

        public Task HandleAsync(ReviewArchivedIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new ArchiveReportRegistrationInternalCommand(@event.ReviewGuid));
        }
    }
}