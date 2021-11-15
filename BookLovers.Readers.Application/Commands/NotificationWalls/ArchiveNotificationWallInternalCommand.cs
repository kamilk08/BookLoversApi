using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class ArchiveNotificationWallInternalCommand : ICommand, IInternalCommand
    {
        public Guid NotificationWallGuid { get; private set; }

        public Guid Guid { get; private set; }

        private ArchiveNotificationWallInternalCommand()
        {
        }

        public ArchiveNotificationWallInternalCommand(Guid notificationWallGuid)
        {
            NotificationWallGuid = notificationWallGuid;
            Guid = Guid.NewGuid();
        }
    }
}