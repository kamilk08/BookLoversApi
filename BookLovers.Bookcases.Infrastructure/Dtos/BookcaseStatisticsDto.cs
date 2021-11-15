namespace BookLovers.Bookcases.Infrastructure.Dtos
{
    public class BookcaseStatisticsDto
    {
        public int BookcaseId { get; set; }

        public int ReaderId { get; set; }

        public int BooksCount { get; set; }

        public int ShelveCount { get; set; }
    }
}