using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.Integration.Books
{
    internal class BookRemoveFromReadShelfHandler :
        IIntegrationEventHandler<BookRemovedFromReadShelfIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInternalCommandDispatcher _internalCommandDispatcher;

        public BookRemoveFromReadShelfHandler(
            IUnitOfWork unitOfWork,
            IInternalCommandDispatcher internalCommandDispatcher)
        {
            this._unitOfWork = unitOfWork;
            this._internalCommandDispatcher = internalCommandDispatcher;
        }

        public async Task HandleAsync(BookRemovedFromReadShelfIntegrationEvent @event)
        {
            var reader = await this._unitOfWork.GetAsync<Reader>(@event.ReaderGuid);

            var allAddedReviews = reader.GetAllAddedReviews();

            var readerReview = allAddedReviews?.SingleOrDefault(p => p.BookGuid == @event.BookGuid);

            if (readerReview != null)
                await this._internalCommandDispatcher.SendInternalCommandAsync(
                    new RemoveReviewCommand(readerReview.ReviewGuid));
        }
    }
}