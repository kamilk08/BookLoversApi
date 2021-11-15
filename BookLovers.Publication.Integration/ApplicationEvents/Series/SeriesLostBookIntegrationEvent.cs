using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Series
{
    public class SeriesLostBookIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public Guid BookGuid { get; private set; }

        private SeriesLostBookIntegrationEvent()
        {
        }

        public SeriesLostBookIntegrationEvent(Guid seriesGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.SeriesGuid = seriesGuid;
            this.BookGuid = bookGuid;
        }
    }
}