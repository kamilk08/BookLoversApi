using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddNewFollowerNotificationInternalCommand : ICommand, IInternalCommand
    {
        public Guid NotificationWallGuid { get; private set; }

        public Guid FollowedByGuid { get; private set; }

        public Guid Guid { get; private set; }

        private AddNewFollowerNotificationInternalCommand()
        {
        }

        public AddNewFollowerNotificationInternalCommand(Guid notificationWallGuid, Guid followedByGuid)
        {
            NotificationWallGuid = notificationWallGuid;
            FollowedByGuid = followedByGuid;
            Guid = Guid.NewGuid();
        }
    }
}