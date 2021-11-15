using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Readers.Domain.NotificationWalls.BusinessRules;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;
using BookLovers.Readers.Events.NotificationWalls;
using BookLovers.Readers.Mementos;

namespace BookLovers.Readers.Domain.NotificationWalls
{
    [AllowSnapshot]
    public class NotificationWall :
        EventSourcedAggregateRoot,
        IHandle<NotificationWallCreated>,
        IHandle<NotificationShownByReader>,
        IHandle<NotificationSeenByReader>,
        IHandle<NotificationHiddenByReader>,
        IHandle<NotificationWallArchived>,
        IHandle<NotificationCreated>
    {
        private List<IWallOption> _wallOptions = new List<IWallOption>();
        private List<Notification> _notifications = new List<Notification>();

        public Guid ReaderGuid { get; private set; }

        public IReadOnlyList<IWallOption> WallOptions => _wallOptions;

        public IReadOnlyList<Notification> Notifications => _notifications;

        private NotificationWall()
        {
        }

        public NotificationWall(Guid wallGuid, Guid readerGuid)
        {
            Guid = wallGuid;
            ReaderGuid = ReaderGuid;

            ApplyChange(new NotificationWallCreated(Guid, readerGuid));
        }

        public void AddNotification(Notification notification)
        {
            CheckBusinessRules(new AddNotificationRules(this, notification));

            var dictionary = notification.GetNotificationItems()
                .ToDictionary(
                    k => k.NotificationObjectGuid,
                    v => v.NotificationItemType.Value);

            ApplyChange(NotificationCreated.Create()
                .WithRequired(Guid, notification.Guid, dictionary)
                .WithSubType(notification.NotificationSubType.Value)
                .AppearedAt(notification.AppearedAt)
                .IsVisible(notification.IsVisibleOnWall()));
        }

        public void HideNotification(Notification notification)
        {
            CheckBusinessRules(new HideNotificationRules(this, notification));

            ApplyChange(new NotificationHiddenByReader(Guid, notification.Guid));
        }

        public void ShowNotification(Notification notification)
        {
            CheckBusinessRules(new ShowNotificationRules(this, notification));

            if (!notification.SeenByReader())
                ApplyChange(new NotificationSeenByReader(Guid, notification.Guid, DateTime.UtcNow));

            ApplyChange(new NotificationShownByReader(Guid, notification.Guid));
        }

        public Notification GetNotification(Guid notificationGuid)
        {
            return _notifications.Find(p => p.Guid == notificationGuid);
        }

        public IWallOption GetWallOption(WallOptionType wallOptionType)
        {
            return _wallOptions.Find(
                p => p.Option == wallOptionType);
        }

        public IEnumerable<Notification> GetAllNotSeenNotifications()
        {
            return _notifications.Where(p => !p.SeenByReader()).AsEnumerable();
        }

        private VisibleOnWall ShouldNotificationBeVisible(bool visible)
        {
            return !visible ? VisibleOnWall.No() : VisibleOnWall.Yes();
        }

        private void AddNotificationItems(
            Dictionary<Guid, int> notificationItems,
            Notification notification)
        {
            foreach (var notificationItem in notificationItems)
                notification.AddNotificationItem(
                    new NotificationItem(notificationItem.Key, notificationItem.Value));
        }

        void IHandle<NotificationWallCreated>.Handle(
            NotificationWallCreated @event)
        {
            Guid = @event.AggregateGuid;
            ReaderGuid = @event.ReaderGuid;
            AggregateStatus = AggregateStatus.Active;

            _wallOptions.Add(new HideNewNotification(false));
            _wallOptions.Add(new HideNotificationAboutBook(false));
            _wallOptions.Add(new HideNotificationAboutReview(false));
            _wallOptions.Add(new HideNotificationFromOther(false));
        }

        void IHandle<NotificationCreated>.Handle(NotificationCreated @event)
        {
            var visibleOnWall = ShouldNotificationBeVisible(@event.Visible);
            var notificationSubType = NotificationSubTypes.Get(@event.NotificationSubTypeId);
            var notification =
                new Notification(@event.NotificationGuid, @event.Date, visibleOnWall, notificationSubType);

            AddNotificationItems(@event.NotificationItems, notification);

            _notifications.Add(notification);
        }

        void IHandle<NotificationSeenByReader>.Handle(
            NotificationSeenByReader @event)
        {
            var notification = GetNotification(@event.NotificationGuid);

            notification.MarkAsSeen(@event.SeenAt);

            notification.VisibleOnWall = VisibleOnWall.Yes();
        }

        void IHandle<NotificationHiddenByReader>.Handle(
            NotificationHiddenByReader @event)
        {
            GetNotification(@event.NotificationGuid).VisibleOnWall =
                VisibleOnWall.No();
        }

        void IHandle<NotificationWallArchived>.Handle(
            NotificationWallArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<NotificationShownByReader>.Handle(
            NotificationShownByReader @event)
        {
            GetNotification(@event.NotificationGuid).VisibleOnWall =
                VisibleOnWall.Yes();
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var notificationWallMemento = memento as INotificationWallMemento;

            Guid = notificationWallMemento.AggregateGuid;

            AggregateStatus = AggregateStates.Get(notificationWallMemento.AggregateStatus);
            Version = notificationWallMemento.Version;
            LastCommittedVersion = notificationWallMemento.LastCommittedVersion;
            ReaderGuid = notificationWallMemento.ReaderGuid;

            _notifications = notificationWallMemento.Notifications.ToList();
            _wallOptions = notificationWallMemento.Options.ToList();
        }
    }
}