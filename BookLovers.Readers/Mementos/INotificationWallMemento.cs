using System;
using System.Collections.Generic;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;

namespace BookLovers.Readers.Mementos
{
    public interface INotificationWallMemento : IMemento<NotificationWall>, IMemento
    {
        Guid ReaderGuid { get; }

        IList<Notification> Notifications { get; }

        IList<IWallOption> Options { get; }
    }
}