using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class AddedResourceRemoved : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ResourceGuid { get; private set; }

        private AddedResourceRemoved()
        {
        }

        [JsonConstructor]
        protected AddedResourceRemoved(Guid guid, Guid aggregateGuid, Guid resourceGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ResourceGuid = resourceGuid;
        }

        public AddedResourceRemoved(Guid aggregateGuid, Guid resourceGuid)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            ResourceGuid = resourceGuid;
        }
    }
}