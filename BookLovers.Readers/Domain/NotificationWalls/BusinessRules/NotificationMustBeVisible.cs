using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;

namespace BookLovers.Readers.Domain.NotificationWalls.BusinessRules
{
    internal class NotificationMustBeVisible : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Notification must be visible";
        private readonly Notification _notification;

        public NotificationMustBeVisible(Notification notification)
        {
            _notification = notification;
        }

        public bool IsFulfilled()
        {
            return _notification.VisibleOnWall != null &&
                   _notification.IsVisibleOnWall();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}