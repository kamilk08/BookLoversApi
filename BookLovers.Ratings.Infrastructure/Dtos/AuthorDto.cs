using System;

namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public Guid AuthorGuid { get; set; }

        public double Average { get; set; }
    }
}