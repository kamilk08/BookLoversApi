using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Readers
{
    public class ReaderEmailChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string Email { get; private set; }

        private ReaderEmailChanged()
        {
        }

        [JsonConstructor]
        protected ReaderEmailChanged(Guid guid, Guid aggregateGuid, string email)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            Email = email;
        }

        public ReaderEmailChanged(Guid aggregateGuid, string email)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            Email = email;
        }
    }
}