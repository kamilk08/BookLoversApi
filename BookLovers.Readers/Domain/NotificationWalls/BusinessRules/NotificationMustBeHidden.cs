using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;

namespace BookLovers.Readers.Domain.NotificationWalls.BusinessRules
{
    internal class NotificationMustBeHidden : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Notfication must be hidden";
        private readonly Notification _notification;

        public NotificationMustBeHidden(Notification notification)
        {
            _notification = notification;
        }

        public bool IsFulfilled()
        {
            return _notification.VisibleOnWall != null &&
                   !_notification.IsVisibleOnWall();
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}