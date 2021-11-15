using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Dtos;

namespace BookLovers.Bookcases.Infrastructure.Queries.Bookcases
{
    public class BookcaseByIdQuery : IQuery<BookcaseDto>
    {
        public int BookcaseId { get; set; }

        public BookcaseByIdQuery()
        {
        }

        public BookcaseByIdQuery(int bookcaseId)
        {
            BookcaseId = bookcaseId;
        }
    }
}