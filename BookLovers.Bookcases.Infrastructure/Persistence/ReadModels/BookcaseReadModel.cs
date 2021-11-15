using System;
using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class BookcaseReadModel : IReadModel<BookcaseReadModel>
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int Status { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public IList<ShelfReadModel> Shelves { get; set; }

        public BookcaseReadModel()
        {
            Shelves = new List<ShelfReadModel>();
        }
    }
}