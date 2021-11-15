using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Domain.Publisher;

namespace BookLovers.Ratings.Application.CommandHandlers.Publishers
{
    internal class CreatePublisherHandler : ICommandHandler<CreatePublisherInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreatePublisherHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(CreatePublisherInternalCommand command)
        {
            var publisher = Publisher.Create(new PublisherIdentification(command.PublisherGuid, command.PublisherId));

            await _unitOfWork.CommitAsync(publisher);
        }
    }
}