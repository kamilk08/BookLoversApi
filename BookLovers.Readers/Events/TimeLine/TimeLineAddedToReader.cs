using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.TimeLine
{
    public class TimeLineAddedToReader : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid TimeLineGuid { get; private set; }

        public int TimeLineStatus { get; private set; }

        private TimeLineAddedToReader()
        {
        }

        [JsonConstructor]
        protected TimeLineAddedToReader(
            Guid guid,
            Guid aggregateGuid,
            Guid timeLineGuid,
            int timeLineStatus)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            TimeLineGuid = timeLineGuid;
            TimeLineStatus = timeLineStatus;
        }

        public TimeLineAddedToReader(Guid aggregateGuid, Guid timeLineGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            TimeLineGuid = timeLineGuid;
            TimeLineStatus = AggregateStatus.Active.Value;
        }
    }
}