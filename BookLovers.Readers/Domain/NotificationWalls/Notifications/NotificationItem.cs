using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Readers.Domain.NotificationWalls.Notifications
{
    public class NotificationItem : ValueObject<NotificationItem>
    {
        public Guid NotificationObjectGuid { get; }

        public NotificationItemType NotificationItemType { get; }

        private NotificationItem()
        {
        }

        public NotificationItem(Guid notificationObjectGuid, NotificationItemType notificationItemType)
        {
            NotificationObjectGuid = notificationObjectGuid;
            NotificationItemType = notificationItemType;
        }

        public NotificationItem(Guid notificationObjectGuid, int notificationItemTypeId)
        {
            NotificationObjectGuid = notificationObjectGuid;
            NotificationItemType = NotificationItemTypes.Get(notificationItemTypeId);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + NotificationObjectGuid.GetHashCode();
            hash = (hash * 23) + NotificationItemType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(NotificationItem obj)
        {
            return NotificationItemType == obj.NotificationItemType &&
                   NotificationObjectGuid == obj.NotificationObjectGuid;
        }
    }
}