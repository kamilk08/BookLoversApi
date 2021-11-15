using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Application.Contracts.Conversions;
using BookLovers.Readers.Domain.Favourites;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Integration.IntegrationEvents;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class AddReviewHandler : ICommandHandler<AddReviewCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IReadContextAccessor _readContextAccessor;
        private readonly IInMemoryEventBus _eventBus;
        private readonly ReviewFactory _factory;

        public AddReviewHandler(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IReadContextAccessor readContextAccessor,
            IInMemoryEventBus eventBus,
            ReviewFactory factory)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _readContextAccessor = readContextAccessor;
            _eventBus = eventBus;
            _factory = factory;
        }

        public async Task HandleAsync(AddReviewCommand command)
        {
            var reviewParts = command.WriteModel.ConvertToReviewParts(_contextAccessor.UserGuid);
            var favourite = await _unitOfWork.GetAsync<Favourite>(command.WriteModel.BookGuid);

            if (!favourite.IsActive())
                throw new BusinessRuleNotMetException("Selected book does not exist");

            var review = _factory.Create(reviewParts);

            await _unitOfWork.CommitAsync(review);

            command.WriteModel.ReviewId = _readContextAccessor.GetReadModelId(command.WriteModel.ReviewGuid);

            await _eventBus.Publish(new ReviewAddedByReaderIntegrationEvent(
                review.ReviewIdentification.ReaderGuid,
                review.Guid, review.ReviewIdentification.BookGuid));
        }
    }
}