using System.Collections.Generic;

namespace BookLovers.Ratings.Infrastructure.Dtos
{
    public class ReaderRatingsDto
    {
        public int ReaderId { get; set; }

        public int RatingsCount { get; set; }

        public Dictionary<int, int> GropedRatings { get; set; }
    }
}