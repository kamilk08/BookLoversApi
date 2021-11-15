using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Base.Infrastructure.Services.Files;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Application.Contracts;

namespace BookLovers.Readers.Application.CommandHandlers.Profiles
{
    internal class ChangeAvatarHandler : ICommandHandler<ChangeAvatarCommand>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IResourceSaver _resourceSaver;

        public ChangeAvatarHandler(IHttpContextAccessor contextAccessor, IResourceSaver resourceSaver)
        {
            _contextAccessor = contextAccessor;
            _resourceSaver = resourceSaver;
        }

        public async Task HandleAsync(ChangeAvatarCommand command)
        {
            if (command.WriteModel.HasImage)
                await _resourceSaver.SaveResourceAsync(_contextAccessor.UserGuid, new ResourceInfo(
                    new UploadFile(Convert.FromBase64String(command.WriteModel.Avatar), command.WriteModel.FileName),
                    ResourceType.Image, ResourceOwner.Avatar));
            else
                await _resourceSaver.SaveResourceAsync(
                    _contextAccessor.UserGuid,
                    new ResourceInfo(null, ResourceType.Image, ResourceOwner.Avatar));
        }
    }
}