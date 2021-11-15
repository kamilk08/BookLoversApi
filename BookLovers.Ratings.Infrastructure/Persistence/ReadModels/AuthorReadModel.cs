using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class AuthorReadModel : IReadModel<AuthorReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid AuthorGuid { get; set; }

        public int AuthorId { get; set; }

        public int Status { get; set; }

        public double Average { get; set; }

        public int RatingsCount { get; set; }

        public IList<BookReadModel> Books { get; set; }

        public AuthorReadModel()
        {
            this.Books = new List<BookReadModel>();
        }
    }
}