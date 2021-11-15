using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Series
{
    public class BookSeriesQuery : IQuery<SeriesDto>
    {
        public int BookId { get; set; }

        public BookSeriesQuery()
        {
        }

        public BookSeriesQuery(int bookId)
        {
            this.BookId = bookId;
        }
    }
}