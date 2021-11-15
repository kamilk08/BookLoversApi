using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class PublisherReadModel : IReadModel<PublisherReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid PublisherGuid { get; set; }

        public int PublisherId { get; set; }

        public int Status { get; set; }

        public double Average { get; set; }

        public int RatingsCount { get; set; }

        public IList<BookReadModel> Books { get; set; }

        public IList<PublisherCycleReadModel> PublisherCycles { get; set; }

        public PublisherReadModel()
        {
            this.Books = new List<BookReadModel>();
            this.PublisherCycles = new List<PublisherCycleReadModel>();
        }
    }
}