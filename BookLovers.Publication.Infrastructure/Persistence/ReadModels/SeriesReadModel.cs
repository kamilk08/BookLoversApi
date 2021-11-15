using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.ReadModels
{
    public class SeriesReadModel : IReadModel<SeriesReadModel>, IReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }

        public List<BookReadModel> Books { get; set; }

        public SeriesReadModel()
        {
            this.Books = new List<BookReadModel>();
        }
    }
}