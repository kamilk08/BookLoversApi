using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.Services;

namespace BookLovers.Readers.Application.CommandHandlers.Readers
{
    internal class AddBookResourceHandler : ICommandHandler<AddBookResourceInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ResourceAdder _resourceAdder;

        public AddBookResourceHandler(IUnitOfWork unitOfWork, ResourceAdder resourceAdder)
        {
            _unitOfWork = unitOfWork;
            _resourceAdder = resourceAdder;
        }

        public async Task HandleAsync(AddBookResourceInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            _resourceAdder.AddResource(reader, new AddedBook(command.BookGuid, command.BookId, command.Date));

            await _unitOfWork.CommitAsync(reader);
        }
    }
}