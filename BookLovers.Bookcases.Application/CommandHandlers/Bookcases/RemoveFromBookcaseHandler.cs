using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Integration.IntegrationEvents;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class RemoveFromBookcaseHandler : ICommandHandler<RemoveFromBookcaseCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBookcaseBookAccessor _bookcaseBookAccessor;

        public RemoveFromBookcaseHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            IHttpContextAccessor contextAccessor,
            IBookcaseBookAccessor bookcaseBookAccessor)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _contextAccessor = contextAccessor;
            _bookcaseBookAccessor = bookcaseBookAccessor;
        }

        public async Task HandleAsync(RemoveFromBookcaseCommand command)
        {
            var bookAggregateGuid =
                await _bookcaseBookAccessor.GetBookcaseBookAggregateGuid(command.WriteModel.BookGuid);
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);
            var bookcaseBook = await _unitOfWork.GetAsync<BookcaseBook>(bookAggregateGuid);

            var list = bookcase.GetShelvesWithBook(bookcaseBook.BookGuid).ToList();
            var shelfOfTypeRead = list.SingleOrDefault(p => p.ShelfDetails.Category == ShelfCategory.Read);

            foreach (var shelf in list)
                bookcase.RemoveFromShelf(bookcaseBook, shelf);

            await _unitOfWork.CommitAsync(bookcase);

            if (shelfOfTypeRead != null)
                await _eventBus.Publish(new BookRemovedFromReadShelfIntegrationEvent(
                    bookcase.Guid,
                    _contextAccessor.UserGuid,
                    command.WriteModel.BookGuid));

            await _eventBus.Publish(new BookRemovedFromBookcaseIntegrationEvent(bookcase.Additions.ReaderGuid));
        }
    }
}