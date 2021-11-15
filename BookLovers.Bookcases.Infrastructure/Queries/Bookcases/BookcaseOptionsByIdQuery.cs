using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcaseOptionsByIdQuery : IQuery<BookcaseOptionsDto>
    {
        public int BookcaseId { get; }

        public BookcaseOptionsByIdQuery(int bookcaseId)
        {
            BookcaseId = bookcaseId;
        }
    }
}