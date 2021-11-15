using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Application.Contracts.Conversions;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Domain.Reviews.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class EditReviewHandler : ICommandHandler<EditReviewCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ReviewEditor _editor;

        public EditReviewHandler(IUnitOfWork unitOfWork, ReviewEditor editor)
        {
            _unitOfWork = unitOfWork;
            _editor = editor;
        }

        public async Task HandleAsync(EditReviewCommand command)
        {
            var review = await _unitOfWork.GetAsync<Review>(command.WriteModel.ReviewGuid);
            var reviewParts = command.WriteModel.ConvertToReviewParts(review.ReviewIdentification.ReaderGuid);

            _editor.EditReview(review, reviewParts);

            await _unitOfWork.CommitAsync(review);
        }
    }
}