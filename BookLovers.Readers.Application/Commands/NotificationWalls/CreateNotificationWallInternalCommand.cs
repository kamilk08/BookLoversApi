using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class CreateNotificationWallInternalCommand : ICommand, IInternalCommand
    {
        public Guid NotificationWallGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public Guid Guid { get; private set; }

        private CreateNotificationWallInternalCommand()
        {
        }

        public CreateNotificationWallInternalCommand(Guid notificationWallGuid, Guid readerGuid)
        {
            NotificationWallGuid = notificationWallGuid;
            ReaderGuid = readerGuid;
            Guid = Guid.NewGuid();
        }
    }
}