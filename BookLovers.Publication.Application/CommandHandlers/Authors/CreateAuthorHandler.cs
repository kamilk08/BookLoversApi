using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.ModuleCommunication;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class CreateAuthorHandler : ICommandHandler<CreateAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInMemoryEventBus _eventBus;
        private readonly AuthorFactory _authorFactory;
        private readonly IResourceSaver _resourceService;
        private readonly IReadContextAccessor _contextAccessor;
        private readonly IBookReaderAccessor _bookReaderAccessor;

        public CreateAuthorHandler(
            IUnitOfWork unitOfWork,
            IInMemoryEventBus eventBus,
            AuthorFactory authorFactory,
            IResourceSaver resourceService,
            IReadContextAccessor contextAccessor,
            IBookReaderAccessor bookReaderAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._eventBus = eventBus;
            this._authorFactory = authorFactory;
            this._resourceService = resourceService;
            this._contextAccessor = contextAccessor;
            this._bookReaderAccessor = bookReaderAccessor;
        }

        public async Task HandleAsync(CreateAuthorCommand command)
        {
            var bookReader =
                await this._bookReaderAccessor.GetAggregateGuidAsync(command.WriteModel.AuthorWriteModel
                    .ReaderGuid);
            var author = this._authorFactory.CreateAuthor(
                command.WriteModel.AuthorWriteModel.ConvertToAuthorData(
                    await this._unitOfWork.GetAsync<BookReader>(bookReader)));

            if (command.WriteModel.PictureWriteModel != null && command.WriteModel.PictureWriteModel.HasImage)
                await this.SaveAuthorsImageAsync(command);

            await this._unitOfWork.CommitAsync(author);

            await this._eventBus.Publish(new NewAuthorAddedIntegrationEvent(
                author.Guid,
                this._contextAccessor.GetReadModelId(author.Guid),
                command.WriteModel.AuthorWriteModel.ReaderGuid));

            command.WriteModel.AuthorWriteModel.AuthorId = this._contextAccessor.GetReadModelId(author.Guid);
        }

        private async Task SaveAuthorsImageAsync(CreateAuthorCommand command)
        {
            await this._resourceService.SaveResourceAsync(
                command.WriteModel.AuthorWriteModel.AuthorGuid,
                new ResourceInfo(
                    new UploadFile(
                        Convert.FromBase64String(command.WriteModel.PictureWriteModel.AuthorImage),
                        command.WriteModel.PictureWriteModel.FileName), ResourceType.Image, ResourceOwner.Author));
        }
    }
}