using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Series
{
    public class SeriesArchivedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid SeriesGuid { get; private set; }

        private SeriesArchivedIntegrationEvent()
        {
        }

        public SeriesArchivedIntegrationEvent(Guid seriesGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.SeriesGuid = seriesGuid;
        }
    }
}