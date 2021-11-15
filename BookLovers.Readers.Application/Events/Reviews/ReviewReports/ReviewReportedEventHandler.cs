using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Events.Reviews;

namespace BookLovers.Readers.Application.Events.Reviews.ReviewReports
{
    internal class ReviewReportedEventHandler :
        IDomainEventHandler<ReviewReported>,
        IDomainEventHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewReportedEventHandler(
            IUnitOfWork unitOfWork,
            IInternalCommandDispatcher commandDispatcher)
        {
            _unitOfWork = unitOfWork;
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReviewReported @event)
        {
            var review = await _unitOfWork.GetAsync<Review>(@event.AggregateGuid);

            if (review.IsLikedBy(@event.ReportedByGuid))
                await _commandDispatcher.SendInternalCommandAsync(
                    new UnLikeReviewInternalCommand(@event.AggregateGuid, @event.ReportedByGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new AddReviewReportedNotificationInternalCommand(@event.ReviewOwnerGuid, @event.ReportedByGuid));
        }
    }
}