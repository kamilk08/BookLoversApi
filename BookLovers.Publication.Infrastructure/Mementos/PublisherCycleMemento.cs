using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mementos
{
    public class PublisherCycleMemento : IMemento<PublisherCycle>, IMemento, ICycleMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public string CycleName { get; private set; }

        [JsonProperty] public Guid PublisherGuid { get; private set; }

        [JsonProperty] public IEnumerable<Guid> CycleBooks { get; private set; }

        public IMemento<PublisherCycle> TakeSnapshot(PublisherCycle aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;

            this.CycleName = aggregate.CycleName;
            this.PublisherGuid = aggregate.PublisherGuid;
            this.CycleBooks = aggregate.Books.Select(s => s.BookGuid).ToList();

            return this;
        }
    }
}