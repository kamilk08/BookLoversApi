using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Mementos
{
    public class SocialProfileMemento : ISocialProfileMemento, IMemento<Profile>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public string Country { get; private set; }

        [JsonProperty] public string CurrentRole { get; private set; }

        [JsonProperty] public Guid ReaderGuid { get; private set; }

        [JsonProperty] public string FirstName { get; private set; }

        [JsonProperty] public string SecondName { get; private set; }

        [JsonProperty] public string City { get; private set; }

        [JsonProperty] public DateTime BirthDate { get; private set; }

        [JsonProperty] public int Sex { get; private set; }

        [JsonProperty] public string WebSite { get; private set; }

        [JsonProperty] public string About { get; private set; }

        [JsonProperty] public DateTime JoinedAt { get; private set; }

        [JsonProperty] public IList<IFavourite> Favourites { get; private set; }

        public IMemento<Profile> TakeSnapshot(Profile aggregate)
        {
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.AggregateGuid = aggregate.Guid;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.Version = aggregate.Version;

            this.ReaderGuid = aggregate.ReaderGuid;
            this.Country = aggregate.Address.Country;
            this.City = aggregate.Address.City;
            this.FirstName = aggregate.Identity.FullName.FirstName;
            this.SecondName = aggregate.Identity.FullName.SecondName;
            this.BirthDate = aggregate.Identity.BirthDate;
            this.Sex = aggregate.Identity.Sex.Value;
            this.CurrentRole = aggregate.CurrentRole.RoleName;
            this.WebSite = aggregate.About.WebSite;
            this.About = aggregate.About.AboutYourself;
            this.JoinedAt = aggregate.About.JoinedAt;

            this.Favourites = aggregate.Favourites.ToList();
            return this;
        }
    }
}