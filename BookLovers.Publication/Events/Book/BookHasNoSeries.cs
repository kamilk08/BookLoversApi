using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookHasNoSeries : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid OldSeriesGuid { get; private set; }

        private BookHasNoSeries()
        {
        }

        public BookHasNoSeries(Guid aggregateGuid, Guid oldSeriesGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.OldSeriesGuid = oldSeriesGuid;
        }
    }
}