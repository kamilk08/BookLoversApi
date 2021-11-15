using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Reviews
{
    public class ReviewMarkToggledByReader : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public bool MarkedAsASpoiler { get; private set; }

        private ReviewMarkToggledByReader()
        {
        }

        [JsonConstructor]
        protected ReviewMarkToggledByReader(Guid guid, Guid aggregateGuid, bool markedAsASpoiler)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            MarkedAsASpoiler = markedAsASpoiler;
        }

        public ReviewMarkToggledByReader(Guid aggregateGuid, bool isMarked)
        {
            Guid = Guid.NewGuid();
            AggregateGuid = aggregateGuid;
            MarkedAsASpoiler = isMarked;
        }
    }
}