using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes
{
    public class QuoteReadModel : IReadModel<QuoteReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public AuthorReadModel Author { get; set; }

        public int? AuthorId { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public BookReadModel Book { get; set; }

        public int? BookId { get; set; }

        public int QuoteType { get; set; }

        public string Quote { get; set; }

        public DateTime AddedAt { get; set; }

        public int Status { get; set; }

        public IList<QuoteLikeReadModel> QuoteLikes { get; set; }

        public QuoteReadModel()
        {
            this.QuoteLikes = new List<QuoteLikeReadModel>();
        }
    }
}