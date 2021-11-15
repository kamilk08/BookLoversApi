using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls;

namespace BookLovers.Readers.Application.CommandHandlers.NotificationWalls
{
    internal class CreateNotificationWallHandler :
        ICommandHandler<CreateNotificationWallInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateNotificationWallHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateNotificationWallInternalCommand command)
        {
            var notificationWall = new NotificationWall(command.NotificationWallGuid, command.ReaderGuid);

            return _unitOfWork.CommitAsync(notificationWall);
        }
    }
}