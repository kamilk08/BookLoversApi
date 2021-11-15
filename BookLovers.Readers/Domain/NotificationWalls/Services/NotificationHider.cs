using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;

namespace BookLovers.Readers.Domain.NotificationWalls.Services
{
    public class NotificationHider : IDomainService<NotificationWall>
    {
        private readonly List<Func<NotificationType, IWallOption, bool>> _notificationHiders =
            new List<Func<NotificationType, IWallOption, bool>>();

        public NotificationHider()
        {
            _notificationHiders.Add(HideIfAllNotificationShouldBeHidden());
            _notificationHiders.Add(HideIfBookNotificationsShouldBeHidden());
            _notificationHiders.Add(HideIfNotificationsFromOthersShouldBeHidden());
            _notificationHiders.Add(HideIfReviewNotificationsShouldBeHidden());
        }

        public bool ShouldNotificationBeHidden(IWallOption wallOption, NotificationType notification)
        {
            bool flag = false;
            foreach (var notificationHider in _notificationHiders)
            {
                if (notificationHider(notification, wallOption))
                    flag = true;
            }

            return flag;
        }

        private Func<NotificationType, IWallOption, bool> HideIfAllNotificationShouldBeHidden() =>
            (notification, wallOption) =>
                wallOption.Option == WallOptionType.HideNotification
                && wallOption.Enabled;

        private Func<NotificationType, IWallOption, bool> HideIfBookNotificationsShouldBeHidden() =>
            (notification, wallOption) =>
                IsNotificationOfType(NotificationType.Book)(notification) &&
                IfWallOptionIsEnabled()(wallOption);

        private Func<NotificationType, IWallOption, bool> HideIfNotificationsFromOthersShouldBeHidden() =>
            (notification, wallOption) =>
                IsNotificationOfType(NotificationType.Follower)(notification) &&
                IfWallOptionIsEnabled()(wallOption);

        private Func<NotificationType, IWallOption, bool> HideIfReviewNotificationsShouldBeHidden() =>
            (notification, wallOption) =>
                IsNotificationOfType(NotificationType.Review)(notification) &&
                IfWallOptionIsEnabled()(wallOption);

        private Func<NotificationType, bool> IsNotificationOfType(
            NotificationType type)
        {
            return notificationType => notificationType == type;
        }

        private Func<IWallOption, bool> IfWallOptionIsEnabled() =>
            wallOption => wallOption.Enabled;
    }
}