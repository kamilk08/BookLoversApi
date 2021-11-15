using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class RemoveReviewHandler :
        ICommandHandler<RemoveReviewCommand>,
        ICommandHandler<RemoveReviewInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<Review> _aggregateManager;
        private readonly IInMemoryEventBus _eventBus;

        public RemoveReviewHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<Review> aggregateManager,
            IInMemoryEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _aggregateManager = aggregateManager;
            _eventBus = eventBus;
        }

        public async Task HandleAsync(RemoveReviewCommand command)
        {
            await ArchiveReviewAsync(command.ReviewGuid);
        }

        public async Task HandleAsync(RemoveReviewInternalCommand command)
        {
            await ArchiveReviewAsync(command.ReviewGuid);
        }

        private async Task ArchiveReviewAsync(Guid reviewGuid)
        {
            var review = await _unitOfWork.GetAsync<Review>(reviewGuid);

            _aggregateManager.Archive(review);

            await _unitOfWork.CommitAsync(review);

            await _eventBus.Publish(new ReviewArchivedIntegrationEvent(
                review.Guid,
                review.ReviewIdentification.BookGuid, review.ReviewIdentification.ReaderGuid));
        }
    }
}