using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class ChangeBookPublisherHandler : ICommandHandler<ChangeBookPublisherInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChangeBookPublisherHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ChangeBookPublisherInternalCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.BookGuid);

            book.ChangePublisher(new BookPublisher(command.PublisherGuid));

            await this._unitOfWork.CommitAsync(book, false);
        }
    }
}