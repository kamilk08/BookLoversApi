using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Entity;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class Notification : IEntityObject
    {
        private readonly List<NotificationItem> _notificationItems = new List<NotificationItem>();

        public Guid Guid { get; private set; }

        public DateTime AppearedAt { get; private set; }

        internal VisibleOnWall VisibleOnWall { get; set; }

        internal NotificationSeen NotificationSeen { get; set; }

        public NotificationSubType NotificationSubType { get; }

        private Notification()
        {
        }

        public Notification(
            Guid guid,
            DateTime appearedAt,
            VisibleOnWall visibleOnWall,
            NotificationSubType notificationSubType)
        {
            Guid = guid;
            AppearedAt = appearedAt;
            VisibleOnWall = visibleOnWall;
            NotificationSeen = NotificationSeen.NotSeen();
            NotificationSubType = notificationSubType;
        }

        public static Notification Create(
            NotificationSubType subType,
            VisibleOnWall visibleOnWall)
        {
            return new Notification(Guid.NewGuid(), DateTime.UtcNow,
                visibleOnWall, subType);
        }

        public void MarkAsSeen(DateTime time)
        {
            NotificationSeen = NotificationSeen.Seen(time);
        }

        public void MarkAsNotSeen()
        {
            NotificationSeen = NotificationSeen.NotSeen();
        }

        public bool IsVisibleOnWall()
        {
            return VisibleOnWall.NotificationState == NotificationState.Visible;
        }

        public bool SeenByReader()
        {
            return NotificationSeen.SeenByReader;
        }

        public IEnumerable<NotificationItem> GetNotificationItems()
        {
            return _notificationItems.AsEnumerable();
        }

        internal void AddNotificationItem(NotificationItem notificationItem)
        {
            _notificationItems.Add(notificationItem);
        }

        public override bool Equals(object obj)
        {
            return obj is Notification notification
                   && Guid == notification.Guid;
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }
    }
}