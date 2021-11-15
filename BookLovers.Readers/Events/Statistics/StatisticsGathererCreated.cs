using System;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Readers.Events.Statistics
{
    public class StatisticsGathererCreated : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ProfileGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        private StatisticsGathererCreated()
        {
        }

        [JsonConstructor]
        protected StatisticsGathererCreated(
            Guid guid,
            Guid aggregateGuid,
            Guid profileGuid,
            Guid readerGuid)
        {
            Guid = guid;
            AggregateGuid = aggregateGuid;
            ProfileGuid = profileGuid;
            ReaderGuid = readerGuid;
        }

        public StatisticsGathererCreated(Guid aggregateGuid, Guid profileGuid, Guid readerGuid)
        {
            AggregateGuid = aggregateGuid;
            ProfileGuid = profileGuid;
            ReaderGuid = readerGuid;
        }
    }
}