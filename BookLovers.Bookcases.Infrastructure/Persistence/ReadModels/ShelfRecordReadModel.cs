using System;

namespace BookLovers.Bookcases.Infrastructure.Persistence.ReadModels
{
    public class ShelfRecordReadModel
    {
        public int Id { get; set; }

        public BookReadModel Book { get; set; }

        public int BookId { get; set; }

        public ShelfReadModel Shelf { get; set; }

        public int ShelfId { get; set; }

        public DateTime? AddedAt { get; set; }
    }
}