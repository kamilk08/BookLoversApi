using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls;

namespace BookLovers.Readers.Application.CommandHandlers.NotificationWalls
{
    internal class ArchiveNotificationWallHandler :
        ICommandHandler<ArchiveNotificationWallInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<NotificationWall> _manager;

        public ArchiveNotificationWallHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<NotificationWall> manager)
        {
            _unitOfWork = unitOfWork;
            _manager = manager;
        }

        public async Task HandleAsync(ArchiveNotificationWallInternalCommand command)
        {
            var notificationWall = await _unitOfWork.GetAsync<NotificationWall>(command.NotificationWallGuid);

            _manager.Archive(notificationWall);

            await _unitOfWork.CommitAsync(notificationWall);
        }
    }
}