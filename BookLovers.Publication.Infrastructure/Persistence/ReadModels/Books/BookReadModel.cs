using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books
{
    public class BookReadModel : IReadModel<BookReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public int CategoryId { get; set; }

        public string SubCategory { get; set; }

        public int SubCategoryId { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Description { get; set; }

        public string DescriptionSource { get; set; }

        public SeriesReadModel Series { get; set; }

        public int? SeriesId { get; set; }

        public int? PositionInSeries { get; set; }

        public PublisherReadModel Publisher { get; set; }

        public int? PublisherId { get; set; }

        public int Pages { get; set; }

        public string Language { get; set; }

        public int LanguageId { get; set; }

        public ReaderReadModel Reader { get; set; }

        public int? ReaderId { get; set; }

        public int Status { get; set; }

        public IList<AuthorReadModel> Authors { get; set; }

        public string CoverSource { get; set; }

        public string CoverType { get; set; }

        public int CoverTypeId { get; set; }

        public IList<ReviewReadModel> Reviews { get; set; }

        public string HashTags { get; set; }

        public IList<QuoteReadModel> Quotes { get; set; }

        public BookReadModel()
        {
            this.Authors = new List<AuthorReadModel>();
            this.Reviews = new List<ReviewReadModel>();
            this.Quotes = new List<QuoteReadModel>();
        }
    }
}