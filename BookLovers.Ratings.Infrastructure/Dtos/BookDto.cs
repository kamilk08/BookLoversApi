using System;
using System.Collections.Generic;

namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public Guid BookGuid { get; set; }

        public double Average { get; set; }

        public IList<RatingDto> Ratings { get; set; }
    }
}