using System;

namespace BookLovers.Bookcases.Infrastructure.Dtos
{
    public class ShelfRecordDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public int ShelfId { get; set; }

        public DateTime AddedAt { get; set; }

        public bool IsActive { get; set; }
    }
}