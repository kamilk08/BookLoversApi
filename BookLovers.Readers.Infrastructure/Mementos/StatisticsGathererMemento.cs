using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Mementos
{
    public class StatisticsGathererMemento :
        IStatisticsGathererMemento,
        IMemento<StatisticsGatherer>,
        IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public Guid ReaderGuid { get; private set; }

        [JsonProperty] public Guid ProfileGuid { get; private set; }

        [JsonProperty] public Dictionary<int, int> Statistics { get; private set; }

        public int GetStatisticValue(StatisticType type) =>
            this.Statistics.ElementAt(type.Value).Value;

        public IMemento<StatisticsGatherer> TakeSnapshot(
            StatisticsGatherer aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.AggregateStatus = aggregate.AggregateStatus.Value;

            this.ReaderGuid = aggregate.ReaderGuid;
            this.ProfileGuid = aggregate.ProfileGuid;
            this.Statistics = aggregate.ToStatisticsDictionary();

            return this;
        }
    }
}