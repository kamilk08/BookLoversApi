using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mementos
{
    public class AuthorMemento : IAuthorMemento, IMemento<Author>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public string FirstName { get; private set; }

        [JsonProperty] public string SecondName { get; private set; }

        [JsonProperty] public DateTime? BirthDate { get; private set; }

        [JsonProperty] public DateTime? DeathDate { get; private set; }

        [JsonProperty] public string BirthPlace { get; private set; }

        [JsonProperty] public string AboutAuthor { get; private set; }

        [JsonProperty] public string AuthorWebsite { get; private set; }

        [JsonProperty] public string DescriptionSource { get; private set; }

        [JsonProperty] public IEnumerable<Guid> AuthorBooks { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Followers { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Quotes { get; private set; }

        [JsonProperty] public IEnumerable<int> Genres { get; private set; }

        [JsonProperty] public int Sex { get; private set; }

        public IMemento<Author> TakeSnapshot(Author aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;

            this.FirstName = aggregate.Basics.FullName.FirstName;
            this.SecondName = aggregate.Basics.FullName.SecondName;
            this.Sex = aggregate.Basics.Sex.Value;
            this.BirthDate = aggregate.Details.BirthDate;
            this.DeathDate = aggregate.Details.DeathDate;
            this.AboutAuthor = aggregate.Description.AboutAuthor;
            this.AuthorWebsite = aggregate.Description.AuthorWebsite;
            this.DescriptionSource = aggregate.Description.DescriptionSource;

            this.AuthorBooks = aggregate.Books.Select(s => s.BookGuid).ToList();
            this.Followers = aggregate.Followers.Select(p => p.FollowedBy).ToList();
            this.Genres = aggregate.Genres.Select(s => s.Value).ToList();
            this.Quotes = aggregate.AuthorQuotes.Select(s => s.QuoteGuid).ToList();

            return this;
        }
    }
}