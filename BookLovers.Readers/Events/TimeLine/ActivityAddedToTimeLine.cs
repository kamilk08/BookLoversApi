using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.TimeLine
{
    public class ActivityAddedToTimeLine : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid TimeLineGuid { get; private set; }

        public Guid ActivityObjectGuid { get; private set; }

        public string Title { get; private set; }

        public DateTime Date { get; private set; }

        public bool ShowToOthers { get; private set; }

        public int ActivityType { get; private set; }

        private ActivityAddedToTimeLine()
        {
        }

        [JsonConstructor]
        protected ActivityAddedToTimeLine(
            Guid guid,
            Guid aggregateGuid,
            Guid timeLineGuid,
            Guid activityObjectGuid,
            string title,
            DateTime date,
            bool showToOthers,
            byte activityType)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            TimeLineGuid = timeLineGuid;
            ActivityObjectGuid = activityObjectGuid;
            Title = title;
            Date = date;
            ShowToOthers = showToOthers;
            ActivityType = activityType;
        }

        private ActivityAddedToTimeLine(
            Guid aggregateGuid,
            Guid activityObjectGuid,
            Guid timeLineGuid,
            string title,
            DateTime date,
            int activityType)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ActivityObjectGuid = activityObjectGuid;
            TimeLineGuid = timeLineGuid;
            Title = title;
            Date = date;
            ShowToOthers = true;
            ActivityType = activityType;
        }

        public static ActivityAddedToTimeLine Initialize()
        {
            return new ActivityAddedToTimeLine();
        }

        public ActivityAddedToTimeLine WithAggregate(Guid aggregateGuid)
        {
            return new ActivityAddedToTimeLine(
                aggregateGuid,
                ActivityObjectGuid, TimeLineGuid,
                Title, Date, ActivityType);
        }

        public ActivityAddedToTimeLine WithTimeLine(Guid timeLineGuid)
        {
            return new ActivityAddedToTimeLine(
                AggregateGuid,
                ActivityObjectGuid, timeLineGuid,
                Title, Date, ActivityType);
        }

        public ActivityAddedToTimeLine WithActivityObject(Guid activityObjectGuid)
        {
            return new ActivityAddedToTimeLine(
                AggregateGuid,
                activityObjectGuid, TimeLineGuid, Title, Date,
                ActivityType);
        }

        public ActivityAddedToTimeLine WithActivity(string title, DateTime date, int activityType)
        {
            return new ActivityAddedToTimeLine(
                AggregateGuid,
                ActivityObjectGuid, TimeLineGuid,
                title, date,
                activityType);
        }
    }
}