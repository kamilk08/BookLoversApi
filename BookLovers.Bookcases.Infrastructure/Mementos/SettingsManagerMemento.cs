using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Bookcases.Infrastructure.Mementos
{
    public class SettingsManagerMemento : ISettingsManagerMemento, IMemento<SettingsManager>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public Guid BookcaseGuid { get; private set; }

        [JsonProperty] public List<IBookcaseOption> SelectedOptions { get; private set; }

        public IMemento<SettingsManager> TakeSnapshot(SettingsManager aggregate)
        {
            Version = aggregate.Version;
            LastCommittedVersion = aggregate.LastCommittedVersion;
            AggregateStatus = aggregate.AggregateStatus.Value;
            AggregateGuid = aggregate.Guid;

            BookcaseGuid = aggregate.BookcaseGuid;
            SelectedOptions = aggregate.Options.ToList();

            return this;
        }
    }
}