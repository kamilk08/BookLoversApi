using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.Services.Editors;
using BookLovers.Publication.Domain.BookReaders;

namespace BookLovers.Publication.Application.CommandHandlers.Authors
{
    internal class EditAuthorHandler : ICommandHandler<EditAuthorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthorEditService _authorEditService;
        private readonly IResourceSaver _resourceService;
        private readonly IBookReaderAccessor _bookReaderAccessor;

        public EditAuthorHandler(
            IUnitOfWork unitOfWork,
            AuthorEditService authorEditService,
            IResourceSaver resourceService,
            IBookReaderAccessor bookReaderAccessor)
        {
            this._unitOfWork = unitOfWork;
            this._authorEditService = authorEditService;
            this._resourceService = resourceService;
            this._bookReaderAccessor = bookReaderAccessor;
        }

        public async Task HandleAsync(EditAuthorCommand command)
        {
            var aggregateGuid =
                await this._bookReaderAccessor.GetAggregateGuidAsync(command.WriteModel.AuthorWriteModel.ReaderGuid);

            var bookReader = await this._unitOfWork.GetAsync<BookReader>(aggregateGuid);

            var author = await this._unitOfWork.GetAsync<Author>(command.WriteModel.AuthorWriteModel.AuthorGuid);

            var authorData = command.WriteModel.AuthorWriteModel
                .ConvertToAuthorData(bookReader);

            await this._authorEditService.EditAuthor(author, authorData);

            if (command.WriteModel.PictureWriteModel.HasImage)
            {
                var resourceInfo =
                    new ResourceInfo(
                        new UploadFile(
                            Convert.FromBase64String(command.WriteModel.PictureWriteModel.AuthorImage),
                            command.WriteModel.PictureWriteModel.FileName), ResourceType.Image, ResourceOwner.Author);
                await this._resourceService.SaveResourceAsync(
                    command.WriteModel.AuthorWriteModel.AuthorGuid,
                    resourceInfo);
            }

            await this._unitOfWork.CommitAsync(author);
        }
    }
}