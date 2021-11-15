using System;
using System.Collections.Generic;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class ShelfReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public BookcaseReadModel Bookcase { get; set; }

        public byte ShelfCategory { get; set; }

        public string ShelfName { get; set; }

        public IList<BookReadModel> Books { get; set; }

        public ShelfReadModel()
        {
            Books = new List<BookReadModel>();
        }
    }
}