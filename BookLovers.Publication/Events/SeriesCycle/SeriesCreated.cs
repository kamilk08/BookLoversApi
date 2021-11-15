using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.SeriesCycle
{
    public class SeriesCreated : IEvent
    {
        public string SeriesName { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int SeriesStatus { get; private set; }

        private SeriesCreated()
        {
        }

        public SeriesCreated(Guid seriesGuid, string name)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = seriesGuid;
            this.SeriesName = name;
            this.SeriesStatus = AggregateStatus.Active.Value;
        }
    }
}