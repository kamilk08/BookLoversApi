using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.NotificationWalls
{
    public class NotificationCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid NotificationGuid { get; private set; }

        public Dictionary<Guid, int> NotificationItems { get; private set; }

        public DateTime Date { get; private set; }

        public bool Visible { get; private set; }

        public bool SeenByReader { get; private set; }

        public DateTime SeenAt { get; private set; }

        public int NotificationSubTypeId { get; private set; }

        private NotificationCreated()
        {
        }

        [JsonConstructor]
        protected NotificationCreated(
            Guid guid,
            Guid aggregateGuid,
            Guid notificationGuid,
            Dictionary<Guid, int> notificationItems,
            DateTime date,
            bool visible,
            bool seenByReader,
            DateTime seenAt,
            int notificationSubTypeId)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            NotificationGuid = notificationGuid;
            NotificationItems = notificationItems;
            Date = date;
            Visible = visible;
            SeenByReader = seenByReader;
            SeenAt = seenAt;
            NotificationSubTypeId = notificationSubTypeId;
        }

        private NotificationCreated(
            Guid aggregateGuid,
            Dictionary<Guid, int> notificationItems,
            Guid notificationGuid,
            DateTime date,
            bool visible,
            int notificationSubTypeId)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            NotificationGuid = notificationGuid;
            NotificationItems = notificationItems;
            Date = date;
            Visible = visible;
            SeenByReader = false;
            SeenAt = default(DateTime);
            NotificationSubTypeId = notificationSubTypeId;
        }

        public static NotificationCreated Create()
        {
            return new NotificationCreated();
        }

        public NotificationCreated WithRequired(
            Guid aggregateGuid,
            Guid notificationGuid,
            Dictionary<Guid, int> notificationItems)
        {
            return new NotificationCreated(aggregateGuid, notificationItems, notificationGuid, Date, Visible,
                NotificationSubTypeId);
        }

        public NotificationCreated WithSubType(int subTypeId)
        {
            return new NotificationCreated(
                AggregateGuid,
                NotificationItems, NotificationGuid, Date, Visible, subTypeId);
        }

        public NotificationCreated AppearedAt(DateTime date)
        {
            return new NotificationCreated(
                AggregateGuid,
                NotificationItems, NotificationGuid, date, Visible, NotificationSubTypeId);
        }

        public NotificationCreated IsVisible(bool visible)
        {
            return new NotificationCreated(AggregateGuid, NotificationItems,
                NotificationGuid, Date, visible, NotificationSubTypeId);
        }
    }
}