using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using Newtonsoft.Json;

namespace BookLovers.Publication.Events.Book
{
    public class BookCreated : IEvent
    {
        public string CategoryName { get; private set; }

        public int BooksCategory { get; private set; }

        public string SubCategoryName { get; private set; }

        [JsonProperty("SubCategory")] public int SubCategoryId { get; private set; }

        public IEnumerable<Guid> BookAuthors { get; private set; }

        public string Isbn { get; private set; }

        public string Title { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public Guid? SeriesGuid { get; private set; }

        public IEnumerable<Guid> Cycles { get; private set; }

        public int? PositionInSeries { get; private set; }

        public int Pages { get; private set; }

        public int LanguageId { get; private set; }

        public string Language { get; private set; }

        public DateTime PublicationDate { get; private set; }

        public double BookRating { get; private set; }

        public int RatingsCount { get; private set; }

        public string BookDescription { get; private set; }

        public string DescriptionSource { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public int BookStatus { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string CoverUrl { get; private set; }

        public string CoverSource { get; private set; }

        public int CoverTypeId { get; private set; }

        public string CoverType { get; private set; }

        public IEnumerable<string> HashTags { get; private set; }

        private BookCreated()
        {
        }

        private BookCreated(
            Guid bookGuid,
            IEnumerable<Guid> authors,
            string title,
            string isbn,
            Guid publisherGuid,
            Guid readerGuid)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = bookGuid;
            this.BookAuthors = authors;
            this.PublisherGuid = publisherGuid;
            this.Title = title;
            this.ReaderGuid = readerGuid;
            this.Isbn = isbn;
            this.BookStatus = AggregateStatus.Active.Value;
        }

        public static BookCreated Initialize()
        {
            return new BookCreated();
        }

        public BookCreated WithAggregate(Guid aggregateGuid)
        {
            return new BookCreated(aggregateGuid, this.BookAuthors,
                this.Title, this.Isbn, this.PublisherGuid, this.ReaderGuid);
        }

        public BookCreated WithAuthors(IEnumerable<Guid> authors)
        {
            return new BookCreated(this.AggregateGuid, authors,
                this.Title, this.Isbn, this.PublisherGuid, this.ReaderGuid);
        }

        public BookCreated WithTitleAndIsbn(string title, string isbn)
        {
            return new BookCreated(
                this.AggregateGuid,
                this.BookAuthors, title, isbn, this.PublisherGuid, this.ReaderGuid);
        }

        public BookCreated WithPublisher(Guid publisherGuid)
        {
            return new BookCreated(this.AggregateGuid, this.BookAuthors,
                this.Title, this.Isbn, publisherGuid, this.ReaderGuid);
        }

        public BookCreated WithAddedBy(Guid readerGuid)
        {
            return new BookCreated(this.AggregateGuid, this.BookAuthors,
                this.Title, this.Isbn, this.PublisherGuid, readerGuid);
        }

        public BookCreated WithSeries(Guid? seriesGuid, int? positionInSeries)
        {
            this.SeriesGuid = seriesGuid.GetValueOrDefault();
            this.PositionInSeries = positionInSeries.GetValueOrDefault();
            return this;
        }

        public BookCreated WithPublicationDate(DateTime? published)
        {
            this.PublicationDate = published.GetValueOrDefault();
            return this;
        }

        public BookCreated WithDetails(int? pages, int? languageId, string language)
        {
            this.Pages = pages.GetValueOrDefault();
            this.LanguageId = languageId.GetValueOrDefault();
            this.Language = language;
            return this;
        }

        public BookCreated WithDescription(string description, string descriptionSource)
        {
            this.BookDescription = description;
            this.DescriptionSource = descriptionSource;
            return this;
        }

        public BookCreated WithCategory(int categoryId, string categoryName)
        {
            this.BooksCategory = categoryId;
            this.CategoryName = categoryName;
            return this;
        }

        public BookCreated WithSubCategory(int subCategoryId, string subCategoryName)
        {
            this.SubCategoryId = subCategoryId;
            this.SubCategoryName = subCategoryName;
            return this;
        }

        public BookCreated WithCover(string coverSource, int coverTypeId, string coverType)
        {
            this.CoverSource = coverSource;
            this.CoverTypeId = coverTypeId;
            this.CoverType = coverType;
            return this;
        }

        public BookCreated WithCycles(List<Guid> cycles)
        {
            this.Cycles = cycles;
            return this;
        }

        public BookCreated WithHashTags(List<string> hashTags)
        {
            this.HashTags = hashTags;
            return this;
        }
    }
}