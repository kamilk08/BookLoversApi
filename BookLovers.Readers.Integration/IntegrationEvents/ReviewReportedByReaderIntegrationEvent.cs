using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Readers.Integration.IntegrationEvents
{
    public class ReviewReportedByReaderIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid ReviewGuid { get; private set; }

        public Guid ReportedByGuid { get; private set; }

        public int ReportReason { get; private set; }

        private ReviewReportedByReaderIntegrationEvent()
        {
        }

        public ReviewReportedByReaderIntegrationEvent(
            Guid reviewGuid,
            Guid reportedByGuid,
            int reportReason)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.ReviewGuid = reviewGuid;
            this.ReportedByGuid = reportedByGuid;
            this.ReportReason = reportReason;
        }
    }
}