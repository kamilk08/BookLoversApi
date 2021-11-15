using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Domain.Books.Services;
using BookLovers.Publication.Integration.ApplicationEvents.Books;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class AddBookHandler : ICommandHandler<AddBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly BookFactory _bookFactory;
        private readonly IResourceSaver _resourcesService;
        private readonly IReadContextAccessor _readContextAccessor;

        public AddBookHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            BookFactory bookFactory,
            IResourceSaver resourcesService,
            IReadContextAccessor readContextAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
            this._bookFactory = bookFactory;
            this._resourcesService = resourcesService;
            this._readContextAccessor = readContextAccessor;
        }

        public async Task HandleAsync(AddBookCommand command)
        {
            var book = await this._bookFactory.CreateBook(command.WriteModel.BookWriteModel.ConvertToBookData());

            if (command.WriteModel.PictureWriteModel != null
                && command.WriteModel.PictureWriteModel.HasImage)
                await this.SaveBookCoverAsync(command);

            await this._unitOfWork.CommitAsync(book);

            var integrationEvent = BookCreatedIntegrationEvent
                .Initialize()
                .WithBook(
                    command.WriteModel.BookWriteModel.BookGuid,
                    this._readContextAccessor.GetReadModelId(book.Guid))
                .WithReader(command.WriteModel.BookWriteModel.AddedBy)
                .WithSeries(book.Series.SeriesGuid.GetValueOrDefault())
                .WithPublisher(book.Publisher.PublisherGuid)
                .WithAuthors(book.Authors.Select(s => s.AuthorGuid))
                .WithCycles(command.WriteModel.BookWriteModel.Cycles);

            await this._eventBus.Publish(integrationEvent);

            command.WriteModel.BookId = integrationEvent.BookId;
        }

        private async Task SaveBookCoverAsync(AddBookCommand command)
        {
            var resourceInfo =
                new ResourceInfo(
                    new UploadFile(
                        Convert.FromBase64String(command.WriteModel.PictureWriteModel.Cover),
                        command.WriteModel.PictureWriteModel.FileName), ResourceType.Image, ResourceOwner.Book);

            await this._resourcesService.SaveResourceAsync(command.WriteModel.BookWriteModel.BookGuid, resourceInfo);
        }
    }
}