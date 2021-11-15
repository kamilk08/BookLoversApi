using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddRemoveFollowerNotificationInternalCommand : ICommand, IInternalCommand
    {
        public Guid NotificationWallGuid { get; private set; }

        public Guid UnFollowedByGuid { get; private set; }

        public Guid Guid { get; private set; }

        private AddRemoveFollowerNotificationInternalCommand()
        {
        }

        public AddRemoveFollowerNotificationInternalCommand(
            Guid notificationWallGuid,
            Guid followedByGuid)
        {
            NotificationWallGuid = notificationWallGuid;
            UnFollowedByGuid = followedByGuid;
            Guid = Guid.NewGuid();
        }
    }
}