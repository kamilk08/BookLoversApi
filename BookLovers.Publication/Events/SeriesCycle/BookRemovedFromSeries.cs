using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.SeriesCycle
{
    public class BookRemovedFromSeries : IEvent
    {
        public Guid BookGuid { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int PositionInSeries { get; private set; }

        private BookRemovedFromSeries()
        {
        }

        public BookRemovedFromSeries(Guid seriesGuid, int positionInSeries, Guid bookGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = seriesGuid;
            this.BookGuid = bookGuid;
            this.PositionInSeries = positionInSeries;
        }
    }
}