using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewReported : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReportedByGuid { get; private set; }

        public Guid ReviewOwnerGuid { get; private set; }

        private ReviewReported()
        {
        }

        [JsonConstructor]
        protected ReviewReported(
            Guid guid,
            Guid aggregateGuid,
            Guid reportedByGuid,
            Guid reviewOwnerGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ReportedByGuid = reportedByGuid;
            ReviewOwnerGuid = reviewOwnerGuid;
        }

        public ReviewReported(Guid aggregateGuid, Guid reportedByGuid, Guid reviewOwnerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ReportedByGuid = reportedByGuid;
            ReviewOwnerGuid = reviewOwnerGuid;
        }
    }
}