using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Ratings.Infrastructure.Persistence.ReadModels
{
    public class SeriesReadModel : IReadModel<SeriesReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid SeriesGuid { get; set; }

        public int SeriesId { get; set; }

        public double Average { get; set; }

        public int RatingsCount { get; set; }

        public int Status { get; set; }

        public IList<BookReadModel> Books { get; set; }

        public SeriesReadModel()
        {
            this.Books = new List<BookReadModel>();
        }
    }
}