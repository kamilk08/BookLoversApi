using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class ShelfByIdQuery : IQuery<ShelfDto>
    {
        public int ShelfId { get; set; }

        public ShelfByIdQuery()
        {
        }

        public ShelfByIdQuery(int shelfId)
        {
            ShelfId = shelfId;
        }
    }
}