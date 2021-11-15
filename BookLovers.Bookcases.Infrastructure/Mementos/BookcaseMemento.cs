using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Infrastructure.Mementos
{
    public class BookcaseMemento : IBookcaseMemento, IMemento<Bookcase>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public Guid ReaderGuid { get; private set; }

        [JsonProperty] public Guid SettingsManagerGuid { get; private set; }

        [JsonProperty] public Guid ShelfRecordTrackerGuid { get; private set; }

        [JsonProperty] public IEnumerable<Shelf> Shelves { get; private set; }

        public IMemento<Bookcase> TakeSnapshot(Bookcase aggregate)
        {
            AggregateGuid = aggregate.Guid;
            AggregateStatus = aggregate.AggregateStatus.Value;
            LastCommittedVersion = aggregate.LastCommittedVersion;
            Version = aggregate.Version;

            ReaderGuid = aggregate.Additions.ReaderGuid;
            SettingsManagerGuid = aggregate.Additions.SettingsManagerGuid;
            ShelfRecordTrackerGuid = aggregate.Additions.ShelfRecordTrackerGuid;
            Shelves = aggregate.Shelves.ToList();

            return this;
        }
    }
}