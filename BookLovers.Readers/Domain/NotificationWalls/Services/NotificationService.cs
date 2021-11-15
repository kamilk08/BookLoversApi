using System.Collections.Generic;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;

namespace BookLovers.Readers.Domain.NotificationWalls.Services
{
    public class NotificationService : IDomainService<NotificationWall>
    {
        private readonly NotificationFactory _notificationFactory;

        public NotificationService(NotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
        }

        public void AddNotification(
            NotificationWall notificationWall,
            List<NotificationItem> notificationItems,
            NotificationSubType type)
        {
            var notification = _notificationFactory.CreateNotification(notificationItems, type, notificationWall.WallOptions);

            notificationWall.AddNotification(notification);
        }
    }
}