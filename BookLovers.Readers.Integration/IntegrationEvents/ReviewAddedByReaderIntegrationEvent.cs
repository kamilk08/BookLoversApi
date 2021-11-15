using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Readers.Integration.IntegrationEvents
{
    public class ReviewAddedByReaderIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private ReviewAddedByReaderIntegrationEvent()
        {
        }

        public ReviewAddedByReaderIntegrationEvent(Guid aggregateGuid, Guid reviewGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.AggregateGuid = aggregateGuid;
            this.ReviewGuid = reviewGuid;
            this.BookGuid = bookGuid;
        }
    }
}