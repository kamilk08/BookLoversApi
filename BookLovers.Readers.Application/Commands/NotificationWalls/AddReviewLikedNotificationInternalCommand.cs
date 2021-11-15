using System;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Readers.Application.Commands.NotificationWalls
{
    internal class AddReviewLikedNotificationInternalCommand : IInternalCommand, ICommand
    {
        public Guid Guid { get; private set; }

        public Guid NotificationWallGuid { get; private set; }

        public Guid LikedByGuid { get; private set; }

        private AddReviewLikedNotificationInternalCommand(Guid notificationWallGuid, Guid likedByGuid)
        {
            Guid = Guid.NewGuid();
            NotificationWallGuid = notificationWallGuid;
            LikedByGuid = likedByGuid;
        }
    }
}