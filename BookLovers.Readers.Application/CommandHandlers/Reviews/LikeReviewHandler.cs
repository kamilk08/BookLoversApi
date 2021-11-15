using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Shared.Likes;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class LikeReviewHandler : ICommandHandler<LikeReviewCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public LikeReviewHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(LikeReviewCommand command)
        {
            var review = await _unitOfWork.GetAsync<Review>(command.ReviewGuid);

            review.AddLike(Like.NewLike(_contextAccessor.UserGuid));

            await _unitOfWork.CommitAsync(review);
        }
    }
}