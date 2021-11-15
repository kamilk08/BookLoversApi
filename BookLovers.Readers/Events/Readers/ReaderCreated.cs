using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid SocialProfileGuid { get; private set; }

        public Guid NotificationWallGuid { get; private set; }

        public Guid StatisticsGathererGuid { get; private set; }

        public int ReaderStatus { get; private set; }

        public Guid TimeLineGuid { get; private set; }

        public string UserName { get; private set; }

        public int ReaderId { get; private set; }

        public string Email { get; private set; }

        private ReaderCreated()
        {
        }

        [JsonConstructor]
        protected ReaderCreated(
            Guid guid,
            Guid aggregateGuid,
            Guid socialProfileGuid,
            Guid notificationWallGuid,
            Guid statisticsGathererGuid,
            int readerStatus,
            Guid timeLineGuid,
            string userName,
            int readerId,
            string email)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            SocialProfileGuid = socialProfileGuid;
            NotificationWallGuid = notificationWallGuid;
            StatisticsGathererGuid = statisticsGathererGuid;
            ReaderStatus = readerStatus;
            TimeLineGuid = timeLineGuid;
            UserName = userName;
            ReaderId = readerId;
            Email = email;
        }

        private ReaderCreated(
            Guid aggregateGuid,
            Guid socialProfileGuid,
            Guid notificationWallGuid,
            Guid statisticsGathererGuid,
            Guid timeLineGuid,
            int readerId,
            string userName,
            string email)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            SocialProfileGuid = socialProfileGuid;
            NotificationWallGuid = notificationWallGuid;
            StatisticsGathererGuid = statisticsGathererGuid;
            ReaderStatus = AggregateStatus.Active.Value;
            TimeLineGuid = timeLineGuid;
            ReaderId = readerId;
            UserName = userName;
            Email = email;
        }

        public static ReaderCreated Initialize()
        {
            return new ReaderCreated();
        }

        public ReaderCreated WithAggregate(Guid aggregateGuid)
        {
            return new ReaderCreated(aggregateGuid, SocialProfileGuid, NotificationWallGuid, StatisticsGathererGuid,
                TimeLineGuid, ReaderId, UserName, Email);
        }

        public ReaderCreated WithReader(int readerId, string username, string email)
        {
            return new ReaderCreated(AggregateGuid, SocialProfileGuid, NotificationWallGuid, StatisticsGathererGuid,
                TimeLineGuid, readerId, username, email);
        }

        public ReaderCreated WithSocials(
            Guid socialProfileGuid,
            Guid notificationWallGuid,
            Guid statisticsGathererGuid,
            Guid timeLineGuid)
        {
            return new ReaderCreated(AggregateGuid, socialProfileGuid, notificationWallGuid, statisticsGathererGuid,
                timeLineGuid, ReaderId, UserName, Email);
        }
    }
}