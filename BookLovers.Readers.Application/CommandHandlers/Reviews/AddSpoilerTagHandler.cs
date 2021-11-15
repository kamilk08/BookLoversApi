using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Domain.Reviews;

namespace BookLovers.Readers.Application.CommandHandlers.Reviews
{
    internal class AddSpoilerTagHandler : ICommandHandler<AddSpoilerTagCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public AddSpoilerTagHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(AddSpoilerTagCommand command)
        {
            var review = await _unitOfWork.GetAsync<Review>(command.ReviewGuid);

            review.AddSpoilerTag(new SpoilerTag(_contextAccessor.UserGuid));

            await _unitOfWork.CommitAsync(review);
        }
    }
}