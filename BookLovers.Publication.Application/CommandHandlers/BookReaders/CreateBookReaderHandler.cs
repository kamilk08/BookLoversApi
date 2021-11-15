using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.BookReaders;
using BookLovers.Publication.Domain.BookReaders;

namespace BookLovers.Publication.Application.CommandHandlers.BookReaders
{
    internal class CreateBookReaderHandler : ICommandHandler<CreateBookReaderInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookReaderHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateBookReaderInternalCommand command)
        {
            return this._unitOfWork.CommitAsync(new BookReader(
                Guid.NewGuid(),
                command.ReaderGuid, command.ReaderId));
        }
    }
}