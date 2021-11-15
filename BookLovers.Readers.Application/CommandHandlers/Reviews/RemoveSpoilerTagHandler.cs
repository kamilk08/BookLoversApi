using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class RemoveSpoilerTagHandler : ICommandHandler<RemoveSpoilerTagCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public RemoveSpoilerTagHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(RemoveSpoilerTagCommand command)
        {
            var review = await _unitOfWork.GetAsync<Review>(command.ReviewGuid);

            var spoilerTag = review.GetSpoilerTag(_contextAccessor.UserGuid);

            review.RemoveSpoilerTag(spoilerTag);

            await _unitOfWork.CommitAsync(review);
        }
    }
}