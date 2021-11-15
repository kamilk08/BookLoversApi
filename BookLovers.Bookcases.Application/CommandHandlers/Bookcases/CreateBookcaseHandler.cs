using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Domain.Services;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class CreateBookcaseHandler : ICommandHandler<CreateBookcaseInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookcaseFactory _bookcaseFactory;

        public CreateBookcaseHandler(IUnitOfWork unitOfWork, BookcaseFactory bookcaseFactory)
        {
            _unitOfWork = unitOfWork;
            _bookcaseFactory = bookcaseFactory;
        }

        public Task HandleAsync(CreateBookcaseInternalCommand command)
        {
            var bookcase = _bookcaseFactory.Create(command.BookcaseGuid, command.ReaderGuid,
                command.ReaderId);

            return _unitOfWork.CommitAsync(bookcase);
        }
    }
}