using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookSeriesPositionChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid SeriesGuid { get; private set; }

        public int Position { get; private set; }

        private BookSeriesPositionChanged()
        {
        }

        public BookSeriesPositionChanged(Guid aggregateGuid, Guid seriesGuid, int position)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.SeriesGuid = seriesGuid;
            this.Position = position;
        }
    }
}