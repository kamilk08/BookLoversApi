using System.Collections.Generic;

namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class BookGroupedRatingsDto
    {
        public int BookId { get; set; }

        public IDictionary<int, int> GroupedRatings { get; set; }
    }
}