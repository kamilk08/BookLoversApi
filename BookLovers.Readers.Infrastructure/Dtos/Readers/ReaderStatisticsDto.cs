namespace BookLovers.Readers.Infrastructure.Dtos.Readers
{
    public class ReaderStatisticsDto
    {
        public int ReaderId { get; set; }

        public int ReviewsCount { get; set; }

        public int GivenLikes { get; set; }

        public int ReceivedLikes { get; set; }

        public int Followers { get; set; }

        public int Followings { get; set; }

        public int AddedAuthors { get; set; }

        public int AddedBooks { get; set; }

        public int AddedQuotes { get; set; }

        public int BooksCount { get; set; }

        public int ShelvesCount { get; set; }
    }
}