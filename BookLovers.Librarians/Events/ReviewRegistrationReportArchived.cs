using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Librarians.Events
{
    public class ReviewRegistrationReportArchived : IEvent
    {
        public Guid Guid { get; }

        public Guid AggregateGuid { get; }

        private ReviewRegistrationReportArchived()
        {
        }

        [JsonConstructor]
        protected ReviewRegistrationReportArchived(Guid guid, Guid aggregateGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
        }

        public ReviewRegistrationReportArchived(Guid aggregateGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
        }
    }
}