using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Profile
{
    public class ProfileCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public DateTime JoinedAt { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int Sex { get; private set; }

        public string CurrentRole { get; private set; }

        public int Status { get; private set; }

        private ProfileCreated()
        {
        }

        [JsonConstructor]
        protected ProfileCreated(
            Guid guid,
            Guid aggregateGuid,
            DateTime joinedAt,
            Guid readerGuid,
            int sex,
            string currentRole,
            int status)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            JoinedAt = joinedAt;
            ReaderGuid = readerGuid;
            CurrentRole = currentRole;
            Sex = sex;
            Status = status;
        }

        private ProfileCreated(
            Guid aggregateGuid,
            Guid readerGuid,
            DateTime joinedAt,
            string currentRole)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReaderGuid = readerGuid;
            JoinedAt = joinedAt;
            CurrentRole = currentRole;
            Sex = Shared.SharedSexes.Sex.Hidden.Value;
            Status = AggregateStatus.Active.Value;
        }

        public static ProfileCreated Initialize()
        {
            return new ProfileCreated();
        }

        public ProfileCreated WithAggregate(Guid aggregateGuid)
        {
            return new ProfileCreated(aggregateGuid, ReaderGuid, JoinedAt, CurrentRole);
        }

        public ProfileCreated WithReader(Guid readerGuid)
        {
            return new ProfileCreated(AggregateGuid, readerGuid, JoinedAt, CurrentRole);
        }

        public ProfileCreated WithJoinedAt(DateTime joinedAt)
        {
            return new ProfileCreated(AggregateGuid, ReaderGuid, joinedAt, CurrentRole);
        }

        public ProfileCreated WithCurrentRole(string currentRole)
        {
            return new ProfileCreated(AggregateGuid, ReaderGuid, JoinedAt, currentRole);
        }
    }
}