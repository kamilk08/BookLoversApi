using System.Collections.Generic;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;

namespace BookLovers.Ratings.Infrastructure.Queries.Ratings
{
    public class MultipleReaderRatingsQuery : IQuery<List<RatingDto>>
    {
        public int ReaderId { get; }

        public List<int> BookIds { get; }

        public MultipleReaderRatingsQuery(int readerId, List<int> bookIds)
        {
            this.ReaderId = readerId;
            this.BookIds = bookIds;
        }
    }
}