using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Book
{
    public class BookHashTagsChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public IList<string> HashTags { get; private set; }

        private BookHashTagsChanged()
        {
        }

        public BookHashTagsChanged(Guid aggregateGuid, IList<string> hashTags)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.HashTags = hashTags;
        }
    }
}