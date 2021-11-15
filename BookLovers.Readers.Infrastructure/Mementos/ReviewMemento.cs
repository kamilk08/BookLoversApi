using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Readers.Mementos;
using Newtonsoft.Json;

namespace BookLovers.Readers.Infrastructure.Mementos
{
    public class ReviewMemento : IReviewMemento, IMemento<BookLovers.Readers.Domain.Reviews.Review>, IMemento
    {
        [JsonProperty] public Guid AggregateGuid { get; private set; }

        [JsonProperty] public int Version { get; private set; }

        [JsonProperty] public int LastCommittedVersion { get; private set; }

        [JsonProperty] public int AggregateStatus { get; private set; }

        [JsonProperty] public IEnumerable<Guid> Likes { get; private set; }

        [JsonProperty] public IEnumerable<Guid> ReviewReports { get; private set; }

        [JsonProperty] public IEnumerable<Guid> SpoilerTags { get; private set; }

        [JsonProperty] public Guid BookGuid { get; private set; }

        [JsonProperty] public Guid ReaderGuid { get; private set; }

        [JsonProperty] public string Review { get; private set; }

        [JsonProperty] public DateTime CreatedAt { get; private set; }

        [JsonProperty] public DateTime ReviewCreatedAt { get; private set; }

        [JsonProperty] public DateTime? EditedAt { get; private set; }

        [JsonProperty] public bool MarkedAsSpoilerByReader { get; private set; }

        [JsonProperty] public bool MarkedByOtherReaders { get; private set; }

        public IMemento<BookLovers.Readers.Domain.Reviews.Review> TakeSnapshot(
            BookLovers.Readers.Domain.Reviews.Review aggregate)
        {
            this.AggregateGuid = aggregate.Guid;
            this.Version = aggregate.Version;
            this.LastCommittedVersion = aggregate.LastCommittedVersion;
            this.AggregateStatus = aggregate.AggregateStatus.Value;

            this.BookGuid = aggregate.ReviewIdentification.BookGuid;
            this.ReaderGuid = aggregate.ReviewIdentification.ReaderGuid;
            this.Review = aggregate.ReviewContent.Review;
            this.EditedAt = new DateTime?(aggregate.ReviewContent.EditedDate.GetValueOrDefault());
            this.CreatedAt = aggregate.ReviewContent.ReviewDate;
            this.MarkedByOtherReaders = aggregate.ReviewSpoiler.MarkedByOtherReaders;
            this.MarkedAsSpoilerByReader = aggregate.ReviewSpoiler.MarkedAsSpoilerByReader;

            this.Likes = aggregate.Likes.Select(s => s.ReaderGuid).ToList();
            this.ReviewReports = aggregate.Reports.Select(s => s.ReportedBy).ToList();
            this.SpoilerTags = aggregate.SpoilerTags.Select(s => s.ReaderGuid).ToList();

            return this;
        }
    }
}