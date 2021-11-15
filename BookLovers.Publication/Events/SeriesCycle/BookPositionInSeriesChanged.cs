using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.SeriesCycle
{
    public class BookPositionInSeriesChanged : IEvent
    {
        public Guid BookGuid { get; private set; }

        public int Position { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private BookPositionInSeriesChanged()
        {
        }

        public BookPositionInSeriesChanged(Guid seriesGuid, Guid bookGuid, int position)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = seriesGuid;
            this.BookGuid = bookGuid;
            this.Position = position;
        }
    }
}