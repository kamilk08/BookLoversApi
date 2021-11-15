using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Quotes
{
    public class QuoteUnLiked : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid UnlikedByGuid { get; private set; }

        private QuoteUnLiked()
        {
        }

        public QuoteUnLiked(Guid aggregateGuid, Guid unlikedByGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.UnlikedByGuid = unlikedByGuid;
        }
    }
}