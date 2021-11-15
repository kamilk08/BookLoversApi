using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class ShelfRecordQuery : IQuery<ShelfRecordDto>
    {
        public int BookId { get; set; }

        public int ShelfId { get; set; }

        public ShelfRecordQuery()
        {
        }

        public ShelfRecordQuery(int bookId, int shelfId)
        {
            BookId = bookId;
            ShelfId = shelfId;
        }
    }
}