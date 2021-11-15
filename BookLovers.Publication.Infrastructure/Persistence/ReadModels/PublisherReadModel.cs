using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels
{
    public class PublisherReadModel : IReadModel<PublisherReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Publisher { get; set; }

        public int Status { get; set; }

        public IList<BookReadModel> Books { get; set; }

        public IList<PublisherCycleReadModel> Cycles { get; set; }

        public PublisherReadModel()
        {
            this.Books = new List<BookReadModel>();
            this.Cycles = new List<PublisherCycleReadModel>();
        }
    }
}