using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.BookcaseBooks;
using BookLovers.Bookcases.Domain.BookcaseBooks;

namespace BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks
{
    internal class AddBookcaseBookHandler : ICommandHandler<AddBookcaseBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddBookcaseBookHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public Task HandleAsync(AddBookcaseBookInternalCommand command)
        {
            var book = new BookcaseBook(Guid.NewGuid(), command.BookGuid,
                command.BookId);

            return _unitOfWork.CommitAsync(book);
        }
    }
}