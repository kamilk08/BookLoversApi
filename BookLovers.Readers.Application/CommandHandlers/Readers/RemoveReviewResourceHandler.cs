using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class RemoveReviewResourceHandler : ICommandHandler<RemoveReviewResourceInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveReviewResourceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RemoveReviewResourceInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            reader.RemoveAddedResource(command.ReviewGuid);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}