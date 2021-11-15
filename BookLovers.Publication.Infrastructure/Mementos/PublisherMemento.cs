using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mementos
{
    public class PublisherMemento : IPublisherMemento, IMemento<Publisher>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public string PublisherName { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Books { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Cycles { get; private set; }

        public IMemento<Publisher> TakeSnapshot(Publisher aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;

            this.PublisherName = aggregate.PublisherName;
            this.Books = aggregate.Books.Select(s => s.BookGuid).ToList();
            this.Cycles = aggregate.Cycles.Select(s => s.CycleGuid).ToList();

            return this;
        }
    }
}