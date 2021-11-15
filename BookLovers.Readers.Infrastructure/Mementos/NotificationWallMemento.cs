using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.WallOptions;
using BookLovers.Readers.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Mementos
{
    public class NotificationWallMemento :
        INotificationWallMemento,
        IMemento<NotificationWall>,
        IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public Guid ReaderGuid { get; private set; }

        [JsonProperty] public IList<Notification> Notifications { get; private set; }

        [JsonProperty] public IList<IWallOption> Options { get; private set; }

        public IMemento<NotificationWall> TakeSnapshot(
            NotificationWall aggregate)
        {
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.AggregateGuid = aggregate.Guid;

            this.ReaderGuid = aggregate.ReaderGuid;
            this.Notifications = aggregate.Notifications.ToList();
            this.Options = aggregate.WallOptions.ToList();

            return this;
        }
    }
}