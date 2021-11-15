using System;

namespace BookLovers.Publication.Infrastructure.Dtos
{
    public class ReviewDto
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public byte Status { get; set; }
    }
}