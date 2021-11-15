using System;

namespace BookLovers.Readers.Infrastructure.Persistence.ReadModels
{
    public class StatisticsReadModel
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public ReaderReadModel Reader { get; set; }

        public int ReaderId { get; set; }

        public int ReviewsCount { get; set; }

        public int ReceivedLikes { get; set; }

        public int GivenLikes { get; set; }

        public int ShelvesCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingsCount { get; set; }

        public int BooksCount { get; set; }

        public int AddedAuthors { get; set; }

        public int AddedBooks { get; set; }

        public int AddedQuotes { get; set; }
    }
}