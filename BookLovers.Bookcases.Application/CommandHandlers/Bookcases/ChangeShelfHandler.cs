using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks;
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Application.Contracts;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Integration.IntegrationEvents;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class ChangeShelfHandler : ICommandHandler<ChangeShelfCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookcaseService _bookcaseService;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IBookcaseBookAccessor _accessor;

        public ChangeShelfHandler(
            IUnitOfWork unitOfWork,
            BookcaseService bookcaseService,
            IInMemoryEventBus eventBus,
            IBookcaseBookAccessor accessor)
        {
            _unitOfWork = unitOfWork;
            _bookcaseService = bookcaseService;
            _eventBus = eventBus;
            _accessor = accessor;
        }

        public async Task HandleAsync(ChangeShelfCommand command)
        {
            var bookAggregateGuid = await _accessor.GetBookcaseBookAggregateGuid(command.WriteModel.BookGuid);
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);
            var bookcaseBook = await _unitOfWork.GetAsync<BookcaseBook>(bookAggregateGuid);
            var settingsManager = await _unitOfWork.GetAsync<SettingsManager>(bookcase.Additions.SettingsManagerGuid);

            var oldShelf = bookcase.GetShelf(command.WriteModel.OldShelfGuid);
            var shelf = bookcase.GetShelf(command.WriteModel.NewShelfGuid);

            _bookcaseService.SetBookcaseWithSettings(bookcase, settingsManager);
            _bookcaseService.ChangeShelf(bookcaseBook, oldShelf, shelf);

            await _unitOfWork.CommitAsync(bookcase);

            if (_bookcaseService.IsShelfOfType(oldShelf, ShelfCategory.Read))
            {
                await _eventBus.Publish(new BookRemovedFromReadShelfIntegrationEvent(
                    bookcase.Guid,
                    bookcase.Additions.ReaderGuid,
                    command.WriteModel.BookGuid));
            }
        }
    }
}