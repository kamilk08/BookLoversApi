using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Application.CommandHandlers.BookcaseBooks;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Integration.IntegrationEvents;

namespace BookLovers.Bookcases.Application.CommandHandlers.Bookcases
{
    internal class AddToBookcaseHandler : ICommandHandler<AddToBookcaseCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookcaseService _bookcaseService;
        private readonly IInMemoryEventBus _eventBus;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBookcaseBookAccessor _bookcaseBookAccessor;

        public AddToBookcaseHandler(
            IUnitOfWork unitOfWork,
            BookcaseService bookcaseService,
            IInMemoryEventBus eventBus,
            IHttpContextAccessor contextAccessor,
            IBookcaseBookAccessor bookcaseBookAccessor)
        {
            _unitOfWork = unitOfWork;
            _bookcaseService = bookcaseService;
            _eventBus = eventBus;
            _contextAccessor = contextAccessor;
            _bookcaseBookAccessor = bookcaseBookAccessor;
        }

        public async Task HandleAsync(AddToBookcaseCommand command)
        {
            var bookAggregateGuid =
                await _bookcaseBookAccessor.GetBookcaseBookAggregateGuid(command.WriteModel.BookGuid);
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);
            var bookcaseBook = await _unitOfWork.GetAsync<BookcaseBook>(bookAggregateGuid);
            var settingsManager = await _unitOfWork.GetAsync<SettingsManager>(bookcase.Additions.SettingsManagerGuid);

            var shelf = bookcase.GetShelf(command.WriteModel.ShelfGuid);

            _bookcaseService.SetBookcaseWithSettings(bookcase, settingsManager);
            _bookcaseService.AddBook(bookcaseBook, shelf);

            await _unitOfWork.CommitAsync(bookcase);

            if (_bookcaseService.IsShelfOfType(shelf, ShelfCategory.Read))
            {
                await _eventBus.Publish(BookAddedToBookcaseIntegrationEvent
                    .Initialize().WithAggregate(bookcase.Guid).WithBook(command.WriteModel.BookGuid)
                    .WithShelf(command.WriteModel.ShelfGuid).WithReader(_contextAccessor.UserGuid));
            }
        }
    }
}