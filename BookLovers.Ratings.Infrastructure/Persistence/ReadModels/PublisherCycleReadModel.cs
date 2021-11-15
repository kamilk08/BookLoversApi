using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class PublisherCycleReadModel : IReadModel<PublisherCycleReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid PublisherCycleGuid { get; set; }

        public int PublisherCycleId { get; set; }

        public int Status { get; set; }

        public double Average { get; set; }

        public int RatingsCount { get; set; }

        public IList<BookReadModel> Books { get; set; }

        public PublisherCycleReadModel()
        {
            this.Books = new List<BookReadModel>();
        }
    }
}