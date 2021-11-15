using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;

namespace BookLovers.Readers.Domain.NotificationWalls.BusinessRules
{
    internal class NotificationWallCannotContainDuplicatedNotifications : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "NotificationWall cannot contain duplicated notifications.";

        private readonly NotificationWall _notificationWall;
        private readonly Notification _notification;

        public NotificationWallCannotContainDuplicatedNotifications(
            NotificationWall notificationWall,
            Notification notification)
        {
            _notificationWall = notificationWall;
            _notification = notification;
        }

        public bool IsFulfilled()
        {
            return !_notificationWall.Notifications.Contains(_notification);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}