using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Quotes
{
    public class QuoteLiked : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid AddedBy { get; private set; }

        private QuoteLiked()
        {
        }

        public QuoteLiked(Guid aggregateGuid, Guid addedBy)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.AddedBy = addedBy;
        }
    }
}