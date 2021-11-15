using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Publication.Application.Contracts;
using BookLovers.Publication.Infrastructure.Directories;
using BookLovers.Publication.Infrastructure.Events;

namespace BookLovers.Publication.Infrastructure.Services
{
    internal class AuthorImageService : IResourceService
    {
        private readonly IInfrastructureEventDispatcher _dispatcher;
        private readonly FileService _fileService;

        public ResourceType ResourceType => ResourceType.Image;

        public ResourceOwner ResourceOwner => ResourceOwner.Author;

        public AuthorImageService(IInfrastructureEventDispatcher dispatcher, FileService fileService)
        {
            this._dispatcher = dispatcher;
            this._fileService = fileService;
        }

        public async Task SaveResourceAsync(IResource resource, Guid resourceGuid)
        {
            this._fileService.SaveFile(resource as UploadFile, PublicationsDirectories.AuthorsFilePath(resourceGuid));

            await this._dispatcher.DispatchAsync(
                new AuthorImageSaved(
                    resourceGuid,
                    this._fileService.FileSaver.FilePath,
                    this._fileService.FileSaver.FileName,
                    this._fileService.FileSaver.FileType));
        }
    }
}