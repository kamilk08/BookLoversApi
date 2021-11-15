namespace BookLovers.Publication.Infrastructure.Dtos.Readers
{
    public class ReaderStatisticsDto
    {
        public int ReaderId { get; set; }

        public int AddedBooks { get; set; }

        public int AddedAuthors { get; set; }

        public int AddedQuotes { get; set; }
    }
}