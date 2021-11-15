using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewUnMarkedByOtherReaders : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public bool MarkedByOthers { get; private set; }

        private ReviewUnMarkedByOtherReaders()
        {
        }

        [JsonConstructor]
        protected ReviewUnMarkedByOtherReaders(Guid guid, Guid aggregateGuid, bool markedByOthers)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            MarkedByOthers = markedByOthers;
        }

        public ReviewUnMarkedByOtherReaders(Guid managerGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = managerGuid;
            MarkedByOthers = false;
        }
    }
}