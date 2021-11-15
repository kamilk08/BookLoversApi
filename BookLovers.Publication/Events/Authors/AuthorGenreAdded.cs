using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorGenreAdded : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int SubCategoryId { get; private set; }

        private AuthorGenreAdded()
        {
        }

        public AuthorGenreAdded(Guid aggregateGuid, int subCategoryId)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.SubCategoryId = subCategoryId;
        }
    }
}