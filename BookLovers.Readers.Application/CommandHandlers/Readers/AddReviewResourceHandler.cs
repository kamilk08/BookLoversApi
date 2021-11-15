using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class AddReviewResourceHandler : ICommandHandler<AddReviewResourceInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResourceAdder _resourceAdder;

        public AddReviewResourceHandler(IUnitOfWork unitOfWork, ResourceAdder resourceAdder)
        {
            _unitOfWork = unitOfWork;
            _resourceAdder = resourceAdder;
        }

        public async Task HandleAsync(AddReviewResourceInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            _resourceAdder.AddResource(reader, new ReaderReview(command.ReviewGuid, command.BookGuid, command.Date));

            await _unitOfWork.CommitAsync(reader);
        }
    }
}