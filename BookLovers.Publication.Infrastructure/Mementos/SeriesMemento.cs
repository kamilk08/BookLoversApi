using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mementos
{
    public class SeriesMemento : ISeriesMemento, IMemento<Series>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public string SeriesName { get; private set; }

        [JsonProperty] public IEnumerable<KeyValuePair<int, Guid>> SeriesBooks { get; private set; }

        public IMemento<Series> TakeSnapshot(Series aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;

            this.SeriesName = aggregate.SeriesName;
            this.SeriesBooks = aggregate.Books
                .Select(p => new KeyValuePair<int, Guid>(p.Position, p.BookGuid))
                .ToList();

            return this;
        }
    }
}