using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class AddAuthorResourceHandler : ICommandHandler<AddAuthorResourceInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResourceAdder _resourceAdder;

        public AddAuthorResourceHandler(IUnitOfWork unitOfWork, ResourceAdder resourceAdder)
        {
            _unitOfWork = unitOfWork;
            _resourceAdder = resourceAdder;
        }

        public async Task HandleAsync(AddAuthorResourceInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            _resourceAdder.AddResource(reader, new AddedAuthor(command.AuthorGuid, command.AuthorId, command.Date));

            await _unitOfWork.CommitAsync(reader);
        }
    }
}