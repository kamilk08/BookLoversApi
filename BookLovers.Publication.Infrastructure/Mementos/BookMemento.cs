using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Publication.Infrastructure.Mementos
{
    public class BookMemento : IBookMemento, IMemento<Book>, IMemento
    {
        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Authors { get; private set; }

        [JsonProperty] public IEnumerable<KeyValuePair<Guid, Guid>> BookReviews { get; private set; }

        [JsonProperty] public IEnumerable<string> BookHashTags { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Quotes { get; private set; }

        [JsonProperty] public Guid PublisherGuid { get; private set; }

        [JsonProperty] public Guid? SeriesGuid { get; private set; }

        [JsonProperty] public int? PositionInSeries { get; private set; }

        [JsonProperty] public string Title { get; private set; }

        [JsonProperty] public string Isbn { get; private set; }

        [JsonProperty] public DateTime PublicationDate { get; private set; }

        [JsonProperty] public int BookCategory { get; private set; }

        [JsonProperty] public int BookSubCategory { get; private set; }

        [JsonProperty] public string Description { get; private set; }

        [JsonProperty] public string DescriptionSource { get; private set; }

        [JsonProperty] public string CoverSource { get; private set; }

        [JsonProperty] public int CoverType { get; private set; }

        [JsonProperty] public int Pages { get; private set; }

        [JsonProperty] public int Language { get; private set; }

        [JsonProperty] public double AverageRating { get; private set; }

        [JsonProperty] public int RatingsCount { get; private set; }

        public IMemento<Book> TakeSnapshot(Book aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.AggregateStatus = aggregate.AggregateStatus.Value;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.Version = aggregate.Version;

            this.Authors = aggregate.Authors.Select(s => s.AuthorGuid).ToList();
            this.BookReviews = aggregate.Reviews
                .Select(p => new KeyValuePair<Guid, Guid>(p.ReaderGuid, p.ReviewGuid)).ToList();

            this.PublisherGuid = aggregate.Publisher.PublisherGuid;
            this.SeriesGuid = aggregate.Series.SeriesGuid;
            this.PositionInSeries = aggregate.Series.PositionInSeries;
            this.Title = aggregate.Basics.Title;
            this.Isbn = aggregate.Basics.ISBN;
            this.PublicationDate = aggregate.Basics.PublicationDate;
            this.BookCategory = aggregate.Basics.BookCategory.Category.Value;
            this.BookSubCategory = aggregate.Basics.BookCategory.SubCategory.Value;
            this.Description = aggregate.Description.BookDescription;
            this.DescriptionSource = aggregate.Description.DescriptionSource;
            this.CoverSource = aggregate.Cover.CoverSource;
            this.CoverType = aggregate.Cover.CoverType.Value;
            this.Pages = aggregate.Details.Pages;
            this.Language = aggregate.Details.Language.Value;

            this.Quotes = aggregate.BookQuotes.Select(s => s.QuoteGuid).ToList();

            return this;
        }
    }
}