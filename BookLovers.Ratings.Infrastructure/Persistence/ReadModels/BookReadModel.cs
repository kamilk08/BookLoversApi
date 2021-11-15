using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class BookReadModel : IReadModel<BookReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid BookGuid { get; set; }

        public int BookId { get; set; }

        public int Status { get; set; }

        public double Average { get; set; }

        public int RatingsCount { get; set; }

        public IList<AuthorReadModel> Authors { get; set; }

        public IList<RatingReadModel> Ratings { get; set; }

        public BookReadModel()
        {
            this.Authors = new List<AuthorReadModel>();
            this.Ratings = new List<RatingReadModel>();
        }
    }
}