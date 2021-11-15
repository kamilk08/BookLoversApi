using System;
using System.Collections.Generic;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookData
    {
        public Guid BookGuid { get; private set; }

        public List<Guid> Authors { get; private set; }

        public BookBasicsData BasicsData { get; private set; }

        public BookDetailsData DetailsData { get; private set; }

        public BookDescriptionData DescriptionData { get; private set; }

        public BookCoverData CoverData { get; private set; }

        public BookSeriesData SeriesData { get; private set; }

        public Guid PublisherGuid { get; private set; }

        public List<string> BookHashTags { get; private set; }

        public List<Guid> Cycles { get; private set; }

        public Guid AddedByGuid { get; private set; }

        private BookData(Guid bookGuid, List<Guid> authors)
        {
            this.BookGuid = bookGuid;
            this.Authors = authors;
        }

        public static BookData Initialize(Guid bookGuid, List<Guid> authors)
        {
            return new BookData(bookGuid, authors);
        }

        public BookData WithAuthors(List<Guid> guides)
        {
            this.Authors = guides;
            return this;
        }

        public BookData WithBasics(BookBasicsData basicsData, Guid publisherGuid)
        {
            this.BasicsData = basicsData;
            this.PublisherGuid = publisherGuid;
            return this;
        }

        public BookData WithDetails(BookDetailsData detailsData)
        {
            this.DetailsData = detailsData;
            return this;
        }

        public BookData WithDescription(BookDescriptionData descriptionData)
        {
            this.DescriptionData = descriptionData;
            return this;
        }

        public BookData WithSeries(BookSeriesData seriesData)
        {
            this.SeriesData = seriesData;
            return this;
        }

        public BookData WithCover(BookCoverData coverData)
        {
            this.CoverData = coverData;
            return this;
        }

        public BookData WithHashTags(List<string> hashTags)
        {
            this.BookHashTags = hashTags;
            return this;
        }

        public BookData WithCycles(List<Guid> guids)
        {
            this.Cycles = guids;
            return this;
        }

        public BookData AddedBy(Guid guid)
        {
            this.AddedByGuid = guid;
            return this;
        }
    }
}