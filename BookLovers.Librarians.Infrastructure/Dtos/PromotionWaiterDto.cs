using System;

namespace BookLovers.Librarians.Infrastructure.Dtos
{
    public class PromotionWaiterDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public Guid ReaderGuid { get; set; }

        public int ReaderId { get; set; }

        public byte PromotionStatus { get; set; }
    }
}