using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Ratings.Events
{
    public class BookRatingChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private BookRatingChanged()
        {
        }

        [JsonConstructor]
        protected BookRatingChanged(Guid guid, Guid aggregateGuid)
        {
            this.Guid = guid;
            this.AggregateGuid = aggregateGuid;
        }

        public BookRatingChanged(Guid aggregateGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
        }
    }
}