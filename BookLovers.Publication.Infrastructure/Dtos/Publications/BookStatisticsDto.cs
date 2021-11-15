namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class BookStatisticsDto
    {
        public int BookId { get; set; }

        public double Average { get; set; }

        public int RatingsCount { get; set; }

        public int ReviewsCount { get; set; }

        public int ReadersCount { get; set; }
    }
}