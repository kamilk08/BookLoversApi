using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Application.Commands.ReviewRegisters;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.IntegrationEvents
{
    public class ReviewReportedByReaderHandler :
        IIntegrationEventHandler<ReviewReportedByReaderIntegrationEvent>
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewReportedByReaderHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReviewReportedByReaderIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new AddReviewReportToRegisterInternalCommand(
                    @event.ReviewGuid,
                    @event.ReportedByGuid, @event.ReportReason));
        }
    }
}