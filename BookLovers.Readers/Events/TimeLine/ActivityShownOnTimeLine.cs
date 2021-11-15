using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.TimeLine
{
    public class ActivityShownOnTimeLine : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid TimeLineGuid { get; private set; }

        public Guid TimeLineObjectGuid { get; private set; }

        public DateTime OccuredAt { get; private set; }

        public int ActivityTypeId { get; private set; }

        private ActivityShownOnTimeLine()
        {
        }

        [JsonConstructor]
        protected ActivityShownOnTimeLine(
            Guid guid,
            Guid aggregateGuid,
            Guid timeLineGuid,
            Guid timeLineObjectGuid,
            DateTime occuredAt,
            int activityTypeId)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            TimeLineGuid = timeLineGuid;
            TimeLineObjectGuid = timeLineObjectGuid;
            OccuredAt = occuredAt;
            ActivityTypeId = activityTypeId;
        }

        private ActivityShownOnTimeLine(
            Guid aggregateGuid,
            Guid timeLineGuid,
            Guid timeLineObjectGuid,
            DateTime occuredAt,
            int activityTypeId)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            TimeLineGuid = timeLineGuid;
            TimeLineObjectGuid = timeLineObjectGuid;
            OccuredAt = occuredAt;
            ActivityTypeId = activityTypeId;
        }

        public static ActivityShownOnTimeLine Initialize()
        {
            return new ActivityShownOnTimeLine();
        }

        public ActivityShownOnTimeLine WithAggregate(Guid aggregateGuid)
        {
            return new ActivityShownOnTimeLine(
                aggregateGuid,
                TimeLineGuid, TimeLineObjectGuid, OccuredAt, ActivityTypeId);
        }

        public ActivityShownOnTimeLine WithTimeLine(Guid timeLineGuid)
        {
            return new ActivityShownOnTimeLine(
                AggregateGuid,
                timeLineGuid, TimeLineObjectGuid,
                OccuredAt, ActivityTypeId);
        }

        public ActivityShownOnTimeLine WithItem(
            Guid timeLineObjectGuid,
            DateTime occuredAt)
        {
            return new ActivityShownOnTimeLine(
                AggregateGuid,
                TimeLineGuid, timeLineObjectGuid, occuredAt,
                ActivityTypeId);
        }

        public ActivityShownOnTimeLine WithType(int activityTypeId)
        {
            return new ActivityShownOnTimeLine(
                AggregateGuid,
                TimeLineGuid, TimeLineObjectGuid,
                OccuredAt, activityTypeId);
        }
    }
}