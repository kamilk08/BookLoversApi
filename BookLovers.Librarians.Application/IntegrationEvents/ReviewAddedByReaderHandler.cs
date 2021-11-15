using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Application.Commands.ReviewRegisters;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Librarians.Application.IntegrationEvents
{
    public class ReviewAddedByReaderHandler :
        IIntegrationEventHandler<ReviewAddedByReaderIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewAddedByReaderHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReviewAddedByReaderIntegrationEvent @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(
                new CreateReviewReportRegisterInternalCommand(@event.ReviewGuid));
        }
    }
}