using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.TimeLine
{
    public class ActivityHiddenOnTimeLine : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid TimeLineGuid { get; private set; }

        public Guid TimeLineObjectGuid { get; private set; }

        public DateTime OccuredAt { get; private set; }

        public int ActivityTypeId { get; private set; }

        private ActivityHiddenOnTimeLine()
        {
        }

        [JsonConstructor]
        protected ActivityHiddenOnTimeLine(
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

        private ActivityHiddenOnTimeLine(
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

        public static ActivityHiddenOnTimeLine Initialize()
        {
            return new ActivityHiddenOnTimeLine();
        }

        public ActivityHiddenOnTimeLine WithAggregate(Guid aggregateGuid)
        {
            return new ActivityHiddenOnTimeLine(
                aggregateGuid,
                TimeLineGuid, TimeLineObjectGuid, OccuredAt, ActivityTypeId);
        }

        public ActivityHiddenOnTimeLine WithTimeLine(Guid timeLineGuid)
        {
            return new ActivityHiddenOnTimeLine(
                AggregateGuid,
                timeLineGuid, TimeLineObjectGuid, OccuredAt, ActivityTypeId);
        }

        public ActivityHiddenOnTimeLine WithItem(Guid timeLineObjectGuid, DateTime occuredAt)
        {
            return new ActivityHiddenOnTimeLine(AggregateGuid, TimeLineGuid, timeLineObjectGuid, occuredAt,
                ActivityTypeId);
        }

        public ActivityHiddenOnTimeLine WithType(int activityTypeId)
        {
            return new ActivityHiddenOnTimeLine(
                AggregateGuid,
                TimeLineGuid, TimeLineObjectGuid,
                OccuredAt, activityTypeId);
        }
    }
}