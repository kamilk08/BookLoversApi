using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.SeriesCycle
{
    public class SeriesArchived : IStateEvent, IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int SeriesStatus { get; private set; }

        public IEnumerable<Guid> Books { get; private set; }

        private SeriesArchived()
        {
        }

        public SeriesArchived(Guid seriesGuid, IEnumerable<Guid> books)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = seriesGuid;
            this.SeriesStatus = AggregateStatus.Archived.Value;
            this.Books = books;
        }
    }
}