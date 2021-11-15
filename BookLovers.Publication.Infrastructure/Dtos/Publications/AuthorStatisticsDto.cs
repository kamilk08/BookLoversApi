namespace BookLovers.Publication.Infrastructure.Dtos.Publications
{
    public class AuthorStatisticsDto
    {
        public int AuthorId { get; set; }

        public double BooksAverage { get; set; }

        public int RatingsCount { get; set; }

        public int ReviewsCount { get; set; }

        public int BooksCount { get; set; }
    }
}