using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.Services;

namespace BookLovers.Publication.Application.CommandHandlers.Books
{
    internal class EditBookHandler : ICommandHandler<EditBookCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BookEditService _bookEditService;
        private readonly IResourceSaver _resourcesService;

        public EditBookHandler(
            IUnitOfWork unitOfWork,
            BookEditService bookEditService,
            IResourceSaver resourcesService)
        {
            this._unitOfWork = unitOfWork;
            this._bookEditService = bookEditService;
            this._resourcesService = resourcesService;
        }

        public async Task HandleAsync(EditBookCommand command)
        {
            var book = await this._unitOfWork.GetAsync<Book>(command.WriteModel.BookWriteModel.BookGuid);
            var bookData = command.WriteModel.BookWriteModel.ConvertToBookData();

            await this._bookEditService.EditBook(book, bookData);

            if (command.WriteModel.PictureWriteModel != null && command.WriteModel.PictureWriteModel.HasImage)
                await this.SaveBookCoverAsync(command);

            await this._unitOfWork.CommitAsync(book);
        }

        private async Task SaveBookCoverAsync(EditBookCommand command)
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