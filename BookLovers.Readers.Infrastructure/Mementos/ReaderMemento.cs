using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;
using BookLovers.Readers.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Mementos
{
    public class ReaderMemento : IReaderMemento, IMemento<Reader>, IMemento
    {
        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public string UserName { get; private set; }

        [JsonProperty] public string Email { get; private set; }

        [JsonProperty] public int ReaderId { get; private set; }

        [JsonProperty] public Guid SocialProfileGuid { get; private set; }

        [JsonProperty] public Guid NotificationWallGuid { get; private set; }

        [JsonProperty] public Guid StatisticsGathererGuid { get; private set; }

        [JsonProperty] public IList<Guid> Followers { get; private set; }

        [JsonProperty] public Guid TimeLineGuid { get; private set; }

        [JsonProperty] public IList<IAddedResource> AddedResources { get; private set; }

        [JsonProperty] public IList<Activity> Activities { get; private set; }

        public IMemento<Reader> TakeSnapshot(Reader aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;

            this.ReaderId = aggregate.Identification.ReaderId;
            this.UserName = aggregate.Identification.Username;
            this.Email = aggregate.Identification.Email;
            this.SocialProfileGuid = aggregate.Socials.ProfileGuid;
            this.NotificationWallGuid = aggregate.Socials.NotificationWallGuid;
            this.StatisticsGathererGuid = aggregate.Socials.StatisticsGathererGuid;
            this.TimeLineGuid = aggregate.TimeLine.Guid;

            this.Activities = aggregate.TimeLine.TimeLineActivities.ToList();
            this.Followers = aggregate.Followers.Select(s => s.FollowedBy).ToList();
            this.AddedResources = aggregate.AddedResources.ToList();

            return this;
        }
    }
}