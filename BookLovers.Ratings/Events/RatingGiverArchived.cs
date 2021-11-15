using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Ratings.Events
{
    public class RatingGiverArchived : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public int ReaderId { get; private set; }

        public List<int> BookIds { get; private set; }

        private RatingGiverArchived()
        {
        }

        [JsonConstructor]
        protected RatingGiverArchived(Guid guid, Guid aggregateGuid, int readerId, List<int> bookIds)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
            this.ReaderId = readerId;
            this.BookIds = bookIds;
        }

        public RatingGiverArchived(Guid aggregateGuid, int readerId, List<int> bookIds)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.ReaderId = readerId;
            this.BookIds = bookIds;
        }
    }
}