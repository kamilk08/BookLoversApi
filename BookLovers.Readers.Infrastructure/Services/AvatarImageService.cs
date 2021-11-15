using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Events.InfrastructureEvents;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Readers.Application.Contracts;
using BookLovers.Readers.Infrastructure.Directories;
using BookLovers.Readers.Infrastructure.EventHandlers;

namespace BookLovers.Readers.Infrastructure.Services
{
    public class AvatarImageService : IResourceService
    {
        private readonly IInfrastructureEventDispatcher _dispatcher;
        private readonly FileService _fileService;

        public ResourceType ResourceType => ResourceType.Image;

        public ResourceOwner ResourceOwner => ResourceOwner.Avatar;

        public AvatarImageService(IInfrastructureEventDispatcher dispatcher, FileService fileService)
        {
            this._dispatcher = dispatcher;
            this._fileService = fileService;
        }

        public async Task SaveResourceAsync(IResource resource, Guid resourceGuid)
        {
            if (resource is UploadFile file && file.HasContent())
            {
                var readersDirectory = ModuleDirectories.GetReadersDirectory(resourceGuid);

                this._fileService.SaveFile(file, readersDirectory);

                await this._dispatcher.DispatchAsync(
                    new AvatarChanged(
                        resourceGuid,
                        this._fileService.FileSaver.FilePath, this._fileService.FileSaver.FileName,
                        this._fileService.FileSaver.FileType));
            }
            else
                await this._dispatcher.DispatchAsync(new AvatarChanged(resourceGuid, string.Empty, string.Empty,
                    string.Empty));
        }
    }
}