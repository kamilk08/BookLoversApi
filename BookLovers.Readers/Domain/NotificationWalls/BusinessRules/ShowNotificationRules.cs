using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;

namespace BookLovers.Readers.Domain.NotificationWalls.BusinessRules
{
    internal sealed class ShowNotificationRules : BaseBusinessRule, IBusinessRule
    {
        protected override List<IBusinessRule> FollowingRules { get; } = new List<IBusinessRule>();

        public ShowNotificationRules(
            NotificationWall notificationWall,
            Notification notification)
        {
            FollowingRules.Add(new AggregateMustExist(notificationWall.Guid));
            FollowingRules.Add(new AggregateMustExist(notification.Guid));
            FollowingRules.Add(new AggregateMustBeActive(notificationWall.AggregateStatus.Value));
            FollowingRules.Add(new NotificationMustBeHidden(notification));
        }

        public bool IsFulfilled()
        {
            return !AreFollowingRulesBroken();
        }

        public string BrokenRuleMessage => Message;
    }
}