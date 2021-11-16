using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Integration.IntegrationEvents;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class RemoveFromShelfHandler : ICommandHandler<RemoveFromShelfCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly BookcaseService _service;
        private readonly IBookcaseBookAccessor _accessor;

        public RemoveFromShelfHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            BookcaseService service,
            IBookcaseBookAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _service = service;
            _accessor = accessor;
        }

        public async Task HandleAsync(RemoveFromShelfCommand command)
        {
            var bookAggregateGuid = await _accessor.GetBookcaseBookAggregateGuid(command.WriteModel.BookGuid);
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);
            var bookcaseBook = await _unitOfWork.GetAsync<BookcaseBook>(bookAggregateGuid);

            var shelf = bookcase.GetShelf(command.WriteModel.ShelfGuid);
            bookcase.RemoveFromShelf(bookcaseBook, shelf);

            await _unitOfWork.CommitAsync(bookcase);

            if (_service.IsShelfOfType(shelf, ShelfCategory.Read))
            {
                var @event = new BookRemovedFromReadShelfIntegrationEvent(
                    bookcase.Guid,
                    bookcase.Additions.ReaderGuid,
                    command.WriteModel.BookGuid);

                await _eventBus.Publish(@event);
            }
        }
    }
}