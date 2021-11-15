using System.Collections.Generic;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;

namespace BookLovers.Readers.Domain.NotificationWalls.Services
{
    public class NotificationFactory
    {
        private readonly NotificationHider _hider;

        public NotificationFactory(NotificationHider hider)
        {
            _hider = hider;
        }

        public Notification CreateNotification(
            IEnumerable<NotificationItem> notificationItems,
            NotificationSubType type,
            IEnumerable<IWallOption> wallOptions)
        {
            bool flag = false;

            foreach (var wallOption in wallOptions)
            {
                if (_hider.ShouldNotificationBeHidden(wallOption, type.NotificationType))
                    flag = true;
            }

            var visibleOnWall = flag ? VisibleOnWall.No() : VisibleOnWall.Yes();

            var notification = Notification.Create(type, visibleOnWall);

            foreach (var notificationItem in notificationItems)
                notification.AddNotificationItem(notificationItem);

            return notification;
        }
    }
}