using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels
{
    public class PublisherCycleReadModel : IReadModel<PublisherCycleReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public PublisherReadModel Publisher { get; set; }

        public int? PublisherId { get; set; }

        public string Cycle { get; set; }

        public int Status { get; set; }

        public IList<BookReadModel> CycleBooks { get; set; }

        public PublisherCycleReadModel()
        {
            this.CycleBooks = new List<BookReadModel>();
        }
    }
}