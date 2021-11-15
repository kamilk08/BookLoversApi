using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class UnLikeReviewHandler :
        ICommandHandler<UnlikeReviewCommand>,
        ICommandHandler<UnLikeReviewInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public UnLikeReviewHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(UnlikeReviewCommand command)
        {
            await UnLikeReviewAsync(command.ReviewGuid, _contextAccessor.UserGuid);
        }

        public async Task HandleAsync(UnLikeReviewInternalCommand command)
        {
            await UnLikeReviewAsync(command.ReviewGuid, command.UserGuid);
        }

        private async Task UnLikeReviewAsync(Guid reviewGuid, Guid userGuid)
        {
            var review = await _unitOfWork.GetAsync<Review>(reviewGuid);

            var like = review.GetLike(userGuid);

            review.RemoveLike(like);

            await _unitOfWork.CommitAsync(review);
        }
    }
}