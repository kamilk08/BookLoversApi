using System;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;

namespace BookLovers.Publication.Integration.ApplicationEvents.Series
{
    public class NewSeriesAddedIntegrationEvent : IIntegrationEvent
    {
        public Guid Guid { get; private set; }

        public DateTime OccuredOn { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public int SeriesId { get; private set; }

        private NewSeriesAddedIntegrationEvent()
        {
        }

        public NewSeriesAddedIntegrationEvent(Guid seriesGuid, int seriesId)
        {
            this.Guid = Guid.NewGuid();
            this.OccuredOn = DateTime.UtcNow;
            this.SeriesGuid = seriesGuid;
            this.SeriesId = seriesId;
        }
    }
}