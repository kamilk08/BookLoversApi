using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Events.NotificationWalls;

namespace BookLovers.Readers.Domain.NotificationWalls.Services
{
    internal class NotificationWallManager : IAggregateManager<NotificationWall>
    {
        private readonly List<Func<NotificationWall, IBusinessRule>> _rules =
            new List<Func<NotificationWall, IBusinessRule>>();

        public NotificationWallManager()
        {
            _rules.Add(notificationWall =>
                new AggregateMustBeActive(notificationWall.AggregateStatus.Value));
        }

        public void Archive(NotificationWall aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new NotificationWallArchived(aggregate.Guid));
        }
    }
}