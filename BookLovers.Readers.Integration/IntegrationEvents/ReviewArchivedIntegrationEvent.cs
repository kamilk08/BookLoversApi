using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Readers.Integration.IntegrationEvents
{
    public class ReviewArchivedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private ReviewArchivedIntegrationEvent()
        {
        }

        public ReviewArchivedIntegrationEvent(Guid reviewGuid, Guid bookGuid, Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReviewGuid = reviewGuid;
            this.BookGuid = bookGuid;
            this.ReaderGuid = readerGuid;
        }
    }
}