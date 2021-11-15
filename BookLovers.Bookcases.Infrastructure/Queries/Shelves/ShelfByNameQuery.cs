using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Shelves
{
    public class ShelfByNameQuery : IQuery<ShelfDto>
    {
        public string ShelfName { get; }

        public int BookcaseId { get; }

        public ShelfByNameQuery(int bookcaseId, string shelfName)
        {
            BookcaseId = bookcaseId;
            ShelfName = shelfName;
        }
    }
}