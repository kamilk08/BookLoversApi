using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewMarkedByOtherReaders : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public bool MarkedAsSpoilerByOthers { get; private set; }

        private ReviewMarkedByOtherReaders()
        {
        }

        [JsonConstructor]
        protected ReviewMarkedByOtherReaders(
            Guid guid,
            Guid aggregateGuid,
            bool markedAsSpoilerByOthers)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            MarkedAsSpoilerByOthers = markedAsSpoilerByOthers;
        }

        public ReviewMarkedByOtherReaders(Guid managerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = managerGuid;
            MarkedAsSpoilerByOthers = true;
        }
    }
}