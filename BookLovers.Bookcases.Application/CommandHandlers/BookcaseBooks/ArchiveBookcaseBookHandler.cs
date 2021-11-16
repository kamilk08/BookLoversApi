using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.BookcaseBooks;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Domain.BookcaseBooks;

namespace BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks
{
    internal class ArchiveBookcaseBookHandler : ICommandHandler<ArchiveBookcaseBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<BookcaseBook> _manager;
        private readonly IBookcaseBookAccessor _accessor;

        public ArchiveBookcaseBookHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<BookcaseBook> manager,
            IBookcaseBookAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
            _accessor = accessor;
        }

        public async Task HandleAsync(ArchiveBookcaseBookInternalCommand command)
        {
            var bookGuid = await _accessor.GetBookcaseBookAggregateGuid(command.BookGuid);

            var bookcaseBook = await _unitOfWork.GetAsync<BookcaseBook>(bookGuid);

            _manager.Archive(bookcaseBook);

            await _unitOfWork.CommitAsync(bookcaseBook);
        }
    }
}