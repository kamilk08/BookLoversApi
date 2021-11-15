using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;

namespace BookLovers.Publication.Infrastructure.Queries.Publishers
{
    public class PublisherByBookIdQuery : IQuery<PublisherDto>
    {
        public int BookId { get; set; }

        public PublisherByBookIdQuery()
        {
        }

        public PublisherByBookIdQuery(int bookId)
        {
            this.BookId = bookId;
        }
    }
}