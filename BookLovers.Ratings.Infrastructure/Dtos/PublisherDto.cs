using System;

namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class PublisherDto
    {
        public int Id { get; set; }

        public Guid PublisherGuid { get; set; }

        public int PublisherId { get; set; }

        public double Average { get; set; }
    }
}