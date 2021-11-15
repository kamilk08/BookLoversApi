using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Series
{
    public class SeriesHasNewBookIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid BookGuid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        private SeriesHasNewBookIntegrationEvent()
        {
        }

        public SeriesHasNewBookIntegrationEvent(Guid seriesGuid, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.BookGuid = bookGuid;
            this.SeriesGuid = seriesGuid;
        }
    }
}