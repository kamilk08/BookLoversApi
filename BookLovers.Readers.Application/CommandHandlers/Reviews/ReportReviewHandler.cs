using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class ReportReviewHandler : ICommandHandler<ReportReviewCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IInMemoryEventBus _eventBus;

        public ReportReviewHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IInMemoryEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(ReportReviewCommand command)
        {
            var review = await _unitOfWork.GetAsync<Review>(command.WriteModel.ReviewGuid);

            review.Report(new ReviewReport(_contextAccessor.UserGuid));

            await _unitOfWork.CommitAsync(review);

            await _eventBus.Publish(new ReviewReportedByReaderIntegrationEvent(review.Guid, _contextAccessor.UserGuid,
                command.WriteModel.ReportReasonId));
        }
    }
}