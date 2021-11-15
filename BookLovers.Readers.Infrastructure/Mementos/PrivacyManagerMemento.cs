using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.ProfileManagers;
using BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions;
using BookLovers.Readers.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Mementos
{
    public class PrivacyManagerMemento :
        IPrivacyManagerMemento,
        IMemento<ProfilePrivacyManager>,
        IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public Guid ProfileGuid { get; private set; }

        [JsonProperty] public IList<IPrivacyOption> PrivacyOptions { get; private set; }

        public IMemento<ProfilePrivacyManager> TakeSnapshot(
            ProfilePrivacyManager aggregate)
        {
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.AggregateGuid = aggregate.Guid;

            this.ProfileGuid = aggregate.ProfileGuid;
            this.PrivacyOptions = aggregate.Options.ToList();

            return this;
        }
    }
}