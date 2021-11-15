using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;

namespace BookLovers.Readers.Domain.NotificationWalls.BusinessRules
{
    internal class NotificationWallMustContainSelectedNotification : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Notification wall does not contain selected notification.";

        private readonly NotificationWall _notificationWall;
        private readonly Notification _notification;

        public NotificationWallMustContainSelectedNotification(
            NotificationWall notificationWall,
            Notification notification)
        {
            _notificationWall = notificationWall;
            _notification = notification;
        }

        public bool IsFulfilled()
        {
            return _notificationWall.Notifications.Contains(_notification);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}